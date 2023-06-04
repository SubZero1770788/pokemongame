using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class AppExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection serv,
            IConfiguration con)
        {
            serv.AddCors();
            serv.AddScoped<IToken, Token>();

            return serv;
        }
    }
}