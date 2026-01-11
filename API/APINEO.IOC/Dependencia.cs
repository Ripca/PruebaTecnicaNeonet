using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using APINEO.BLL;
using APINEO.Entities.Models;
using APINEO.DAL;

namespace APINEO.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<EmpresaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConexion"));
            });

            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IEmpresaService, EmpresaService>();
        }
    }
}
