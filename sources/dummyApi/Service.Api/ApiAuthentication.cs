using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class ApiAuthentication : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices.GetService<IAuthenticationService>();

            if (authService == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new ForbidResult();
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var jwtData = authService.GetJwtData();

            if (jwtData == null || string.IsNullOrEmpty(jwtData.SecurityKey))
            {
                context.Result = new ForbidResult();
                return;
            }

            var principal = ValidateJwtToken(token, jwtData);

            if (principal == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            context.HttpContext.User = principal;
        }

        private ClaimsPrincipal? ValidateJwtToken(string token, dynamic jwtModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtModel.SecurityKey);

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidAudience = jwtModel.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
