using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> SignInAsync(SignInRequest request);
        Task<string?> RefreshToken(RefreshTokenRequest request);
        JwtTokenData GetJwtData();
    }
}
