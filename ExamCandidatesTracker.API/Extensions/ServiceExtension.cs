using Exam.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Exam.API.Extensions
{
    public static class ServiceExtension
    {
        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt");
            services.AddAuthentication(v =>
            {
                v.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                v.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(v =>
             {
                 v.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateLifetime = true,
                     ValidateAudience = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtConfig.GetSection("Issuer").Value,
                     ValidAudience = jwtConfig.GetSection("Audience").Value,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")))
                 };
             });
        }

        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<ExamDbContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
