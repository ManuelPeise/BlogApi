using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string?> SignInAsync(SignInRequest request);
        Task<string?> RefreshToken(RefreshTokenRequest request);
        Task<string> ChangePassword(ChangePasswordRequestModel request);
        JwtTokenData GetJwtData();
    }
}
