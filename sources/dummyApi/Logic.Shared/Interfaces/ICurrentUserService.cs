using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        UserModel? CurrentUser { get; }
        Task<UserModel?> GetCurrentUser();
    }
}
