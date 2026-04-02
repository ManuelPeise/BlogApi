using Data.Database;
using Data.Database.Interfaces;
using Logic.Services;
using Logic.Services.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.Text;

namespace Web.Api.Bundels
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, string corsPolicy)
        {
            services.Configure<JwtTokenData>(configuration.GetSection("Jwt"));

            var connectionString = configuration.GetConnectionString("BlogDb");

            if (connectionString == null)
            {
                throw new InvalidOperationException("Connection string 'BlogDb' not found.");
            }

            services.AddDbContext<DatabaseContext>(options =>
                options.UseMySQL(connectionString));

            services.AddScoped(typeof(IDbRepositoryBase<>), typeof(DbRepositoryBase<>));

            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                    policy.WithOrigins("https://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
            });

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtConfig = configuration.GetSection("Jwt").Get<JwtTokenData>();

            if (jwtConfig == null)
            {
                throw new InvalidOperationException("JWT configuration section is missing or invalid.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = jwtConfig?.SecurityKey ?? string.Empty;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                    //OnForbidden = context =>
                    //{
                    //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    //    return Task.CompletedTask;
                    //}
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig?.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    
                };
            });

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddOpenApi();

            Swagger.RegisterSwagger(services);
        }
    }
}
