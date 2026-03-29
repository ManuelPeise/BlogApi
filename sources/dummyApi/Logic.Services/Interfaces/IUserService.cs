using Shared.Models;

namespace Logic.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUsers(bool includeBlog);
        Task<UserModel?> GetUserById(int id, bool includeBlog);
        Task<bool> CreateUser(SignupModel signupModel);
        Task<UserModel?> UpdateUser(UserModel userModel);
        Task DeleteUser(int id);
    }
}
