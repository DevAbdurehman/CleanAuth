using CleanAuth.Application.Interfaces.Repositories;
using CleanAuth.Application.Services.Implementations;
using CleanAuth.Application.Services.Interfaces;
using CleanAuth.Domain.Entities;
using CleanAuth.Infrastructure.Configuration;
using CleanAuth.Infrastructure.Data;
using CleanAuth.Infrastructure.Repositories;
using CleanAuth.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanAuth.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>
                (options => options.UseSqlServer
                (configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepositories>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


            return services;

        }
    }
}
