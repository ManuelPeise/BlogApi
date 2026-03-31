using Data.Database.Entities;
using Data.Database.Interfaces;
using Logic.Services.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Logic.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly JwtTokenData _jwtTokenData;

        public AuthenticationService(
            IUnitOfWork unitOfWork, 
            ILogger<AuthenticationService> logger,
            IOptions<JwtTokenData> tokenOptions)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jwtTokenData = tokenOptions.Value;
        }

        public async Task<string?> SignInAsync(SignInRequest request)
        {
            try
            {
                var userEntity = await _unitOfWork.UserTable.QueryData(false, x => x.Email == request.Email, x => x.Credentials);

                if(userEntity == null && userEntity?.Credentials == null)
                {
                    throw new InvalidOperationException("Could not find user in database");
                }

                if(!PasswordHasher.VerifyPassword(request.Password, userEntity.Credentials.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Sign in failed due to invalid password.");
                }

                userEntity.Credentials.RefreshToken = GenerateRefreshToken();

                await _unitOfWork.SaveChanges();

                return GenerateJwt(userEntity);

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Warning, exception, "User authentication failed");

                return null;

            }
        }

        public async Task<string?> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(request.Token);

                var email = principal.Identity!.Name ?? string.Empty;

                var user = await _unitOfWork.UserTable.QueryData(false, x => x.Email == email, x => x.Credentials);

                if (user == null || user?.Credentials == null || user.Credentials.RefreshToken != request.RefreshToken)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                var newAccessToken = GenerateJwt(user);
                var newRefreshToken = GenerateRefreshToken();

                user.Credentials.RefreshToken = newRefreshToken;

                await _unitOfWork.SaveChanges();

                return newAccessToken;

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Token refresh failed");

                return null;
            }
        }

        public async Task<string> ChangePassword(ChangePasswordRequestModel request)
        {
            try
            {
                var userEntity = await _unitOfWork.UserTable.GetByIdAsync(false, request.UserId, x => x.Credentials);

                if(userEntity == null)
                {
                    throw new Exception($"Could not change password of user {request.UserId}, user not found.");
                }

                if(!PasswordHasher.VerifyPassword(request.CurrentPassword, userEntity.Credentials.PasswordHash))
                {
                    throw new Exception($"Submitted password does not match the current password.");
                }

                userEntity.Credentials.PasswordHash = PasswordHasher.HashPassword(request.UpdatedPassword);

                await _unitOfWork.SaveChanges();

                var jwt = GenerateJwt(userEntity);

                return jwt;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Warning, exception, "Token refresh failed");

                return string.Empty;
            }
        }
        
        public JwtTokenData GetJwtData()
        {
            return _jwtTokenData;
        }

        private string GenerateJwt(UserEntity appUserEntity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenData.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = GetUserClaims(appUserEntity, _jwtTokenData.ExpiresInSeconds);

            var token = new JwtSecurityToken(
                issuer: _jwtTokenData.Issuer,
                audience: _jwtTokenData.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(_jwtTokenData.ExpiresInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private static List<Claim> GetUserClaims(UserEntity user, int expireSeconds)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddSeconds(expireSeconds).ToString("o"))
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtTokenData.Audience,

                ValidateIssuer = true,
                ValidIssuer = _jwtTokenData.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtTokenData.SecurityKey)
                ),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

    }
}
