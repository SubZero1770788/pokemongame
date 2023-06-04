using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection serv,
            IConfiguration con)
        {
            serv.AddIdentityCore<UserData>(o =>
            {
                o.Password.RequiredLength = 7;
                o.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<UserRoles>()
                .AddRoleManager<RoleManager<UserRoles>>()
                .AddEntityFrameworkStores<DataContext>();

            serv.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                    .UTF8.GetBytes(con["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return serv;
        }
    }
}