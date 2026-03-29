using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface ICurrentUserService
    {
        UserModel? CurrentUser { get; }
        Task<UserModel?> GetCurrentUser();
    }
}
