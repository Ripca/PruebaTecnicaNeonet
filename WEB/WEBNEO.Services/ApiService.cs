using System.Net.Http.Json;
using System.Net.Http.Headers;
using WEBNEO.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace WEBNEO.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var baseUrl = _configuration["ApiBaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _httpClient.BaseAddress = new Uri(baseUrl);
            }
        }

        private async Task<T> GetAsync<T>(string url, T defaultValue)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AttachToken(request);

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"[ApiService Error] {url} returned {response.StatusCode}: {errorContent}");
                return defaultValue;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ApiService Exception] {url}: {ex.Message}");
                return defaultValue;
            }
        }

        private void AttachToken(HttpRequestMessage request)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // AUTENTICACIÓN
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<MenuDTO>> ObtenerMenusPorUsuario(int usuarioId)
        {
            return await GetAsync<List<MenuDTO>>($"api/Menus/usuario/{usuarioId}", new List<MenuDTO>());
        }

        // PRODUCTOS
        public async Task<List<Producto>> ListarProductos()
        {
            return await GetAsync<List<Producto>>("api/Productos", new List<Producto>());
        }

        public async Task<Producto> ObtenerProductoPorId(int id)
        {
            return await GetAsync<Producto>($"api/Productos/{id}", null);
        }

        public async Task<ResponseResult> CrearProducto(Producto request)
        {
            return await PostAsync("api/Productos", request, "Producto creado");
        }

        public async Task<ResponseResult> ActualizarProducto(Producto request)
        {
            return await PutAsync("api/Productos", request, "Producto actualizado");
        }

        public async Task<ResponseResult> CambiarEstadoProducto(int id, int estado)
        {
            return await PatchAsync($"api/Productos/{id}/estado/{estado}", "Estado actualizado");
        }

        // CLIENTES
        public async Task<List<Cliente>> ListarClientes()
        {
            return await GetAsync<List<Cliente>>("api/Clientes", new List<Cliente>());
        }

        public async Task<Cliente> ObtenerClientePorId(int id)
        {
            return await GetAsync<Cliente>($"api/Clientes/{id}", null);
        }

        public async Task<Cliente> BuscarClientePorEmail(string email)
        {
            return await GetAsync<Cliente>($"api/Clientes/buscar/{email}", null);
        }

        public async Task<ResponseResult> CrearCliente(Cliente request)
        {
            return await PostAsync("api/Clientes", request, "Cliente creado");
        }

        public async Task<ResponseResult> ActualizarCliente(Cliente request)
        {
            return await PutAsync("api/Clientes", request, "Cliente actualizado");
        }

        public async Task<ResponseResult> CambiarEstadoCliente(int id, int estado)
        {
            return await PatchAsync($"api/Clientes/{id}/estado/{estado}", "Estado actualizado");
        }

        // Acciones
        private async Task<ResponseResult> PostAsync<T>(string url, T data, string successMessage)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                AttachToken(request);
                request.Content = JsonContent.Create(data);
                var response = await _httpClient.SendAsync(request);
                return await ProcessResponse(response, successMessage);
            }
            catch (Exception ex) { return new ResponseResult { Success = false, Message = ex.Message }; }
        }

        private async Task<ResponseResult> PutAsync<T>(string url, T data, string successMessage)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                AttachToken(request);
                request.Content = JsonContent.Create(data);
                var response = await _httpClient.SendAsync(request);
                return await ProcessResponse(response, successMessage);
            }
            catch (Exception ex) { return new ResponseResult { Success = false, Message = ex.Message }; }
        }

        private async Task<ResponseResult> PatchAsync(string url, string successMessage)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, url);
                AttachToken(request);
                var response = await _httpClient.SendAsync(request);
                return await ProcessResponse(response, successMessage);
            }
            catch (Exception ex) { return new ResponseResult { Success = false, Message = ex.Message }; }
        }

        private async Task<ResponseResult> ProcessResponse(HttpResponseMessage response, string successMessage)
        {
            if (response.IsSuccessStatusCode) return new ResponseResult { Success = true, Message = successMessage };
            var body = await response.Content.ReadAsStringAsync();
            return new ResponseResult { Success = false, Message = body };
        }

        // VENTAS
        public async Task<ResponseResult> RegistrarVenta(VentaRequest request)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/Ventas");
                AttachToken(httpRequest);
                httpRequest.Content = JsonContent.Create(request);

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseResult { Success = true, Message = "Venta registrada" };
                }

                var errorBody = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonSerializer.Deserialize<ErrorResponse>(errorBody, _jsonOptions);
                    return new ResponseResult { Success = false, Message = error?.Mensaje ?? "Error al registrar venta" };
                }
                catch
                {
                    return new ResponseResult { Success = false, Message = "Error al registrar venta (Status: " + response.StatusCode + ")" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Success = false, Message = "Error de conexión: " + ex.Message };
            }
        }

        public async Task<List<VentaDTO>> ListarVentas()
        {
            return await GetAsync<List<VentaDTO>>("api/Ventas", new List<VentaDTO>());
        }



        public async Task<List<VentaDTO>> ObtenerVentasPorCliente(int clienteId)
        {
            return await GetAsync<List<VentaDTO>>($"api/Ventas/por-cliente/{clienteId}", new List<VentaDTO>());
        }

        public async Task<List<VentaHistorialDTO>> ObtenerDetalleVenta(int ventaId)
        {
            return await GetAsync<List<VentaHistorialDTO>>($"api/Ventas/detalle/{ventaId}", new List<VentaHistorialDTO>());
        }


        // USUARIOS
        public async Task<List<UsuarioDTO>> ListarUsuarios()
        {
            return await GetAsync<List<UsuarioDTO>>("api/Usuarios", new List<UsuarioDTO>());
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            return await GetAsync<UsuarioDTO>($"api/Usuarios/{id}", null);
        }

        public async Task<ResponseResult> CrearUsuario(UsuarioCreateRequest request)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, "api/Usuarios");
                AttachToken(httpRequest);
                httpRequest.Content = JsonContent.Create(request);

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseResult { Success = true, Message = "Usuario creado exitosamente" };
                }
                var errorBody = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonSerializer.Deserialize<ErrorResponse>(errorBody, _jsonOptions);
                    return new ResponseResult { Success = false, Message = error?.Mensaje ?? "Error al crear usuario" };
                }
                catch
                {
                    return new ResponseResult { Success = false, Message = "Error al crear usuario (Status: " + response.StatusCode + ")" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Success = false, Message = "Error de conexión: " + ex.Message };
            }
        }

        public async Task<ResponseResult> ActualizarUsuario(UsuarioUpdateRequest request)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, "api/Usuarios");
                AttachToken(httpRequest);
                httpRequest.Content = JsonContent.Create(request);

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseResult { Success = true, Message = "Usuario actualizado exitosamente" };
                }
                var errorBody = await response.Content.ReadAsStringAsync();
                try
                {
                    var error = JsonSerializer.Deserialize<ErrorResponse>(errorBody, _jsonOptions);
                    return new ResponseResult { Success = false, Message = error?.Mensaje ?? "Error al actualizar usuario" };
                }
                catch
                {
                    return new ResponseResult { Success = false, Message = "Error al actualizar usuario (Status: " + response.StatusCode + ")" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseResult { Success = false, Message = "Error de conexión: " + ex.Message };
            }
        }

        public async Task<ResponseResult> CambiarEstadoUsuario(int id, int estado)
        {
            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"api/Usuarios/{id}/estado");
                AttachToken(httpRequest);
                httpRequest.Content = JsonContent.Create(estado);

                var response = await _httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseResult { Success = true, Message = "Estado actualizado" };
                }
                return new ResponseResult { Success = false, Message = "Error al cambiar estado" };
            }
            catch
            {
                return new ResponseResult { Success = false, Message = "Error de conexión" };
            }
        }

        // ROLES
        public async Task<List<Rol>> ListarRoles()
        {
            return await GetAsync<List<Rol>>("api/Roles", new List<Rol>());
        }

        // DASHBOARD
        public async Task<List<ImagenDashboard>> ListarImagenesDashboard()
        {
            return await GetAsync<List<ImagenDashboard>>("api/Dashboard/Imagenes", new List<ImagenDashboard>());
        }


    }
}
