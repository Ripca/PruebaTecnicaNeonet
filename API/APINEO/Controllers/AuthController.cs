using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APINEO.BLL;
using APINEO.Entities.Models;

namespace APINEO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly IConfiguration _configuration;

        public AuthController(IEmpresaService empresaService, IConfiguration configuration)
        {
            _empresaService = empresaService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // 1. Validar ReCaptcha
                if (!await VerifyRecaptcha(request.TokenCaptcha))
                {
                    return BadRequest(new { message = "Validación de ReCaptcha fallida" });
                }

                // 2. Validar usuario
                var usuario = await _empresaService.ValidarUsuario(request.Email, request.Password);

                if (usuario == null)
                {
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                // Generar token JWE (Encrypted)
                var token = GenerateJweToken(usuario);

                var response = new LoginResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    RolesIds = usuario.RolesIdsList, // Enviar IDs de roles
                    Token = token
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }
        }

        private async Task<bool> VerifyRecaptcha(string token)
        {
            // Validar si la configuración requiere validación
            var skipValidation = _configuration["ValidarRecaptcha"] == "N";
            if (skipValidation) return true;

            if (string.IsNullOrEmpty(token)) return false;

            try
            {
                using var client = new HttpClient();
                var secret = _configuration["RecaptchaSecretKey"];
                var response = await client.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}",
                    null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<dynamic>();
                    return (bool)result.success;
                }
            }
            catch { }
            return false;
        }

        private string GenerateJweToken(UsuarioDTO usuario)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var encryptionKey = _configuration["Jwt:EncryptionKey"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];
            var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Credenciales de Encriptación (JWE) ✅
            var encryptingCredentials = new EncryptingCredentials(
                encryptKey,
                SecurityAlgorithms.Aes256KeyWrap,
                SecurityAlgorithms.Aes256CbcHmacSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nombre", usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Agregar cada rol como un claim por separado
            foreach (var rolId in usuario.RolesIdsList)
            {
                claims.Add(new Claim("rol_id", rolId.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = credentials,
                EncryptingCredentials = encryptingCredentials 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
