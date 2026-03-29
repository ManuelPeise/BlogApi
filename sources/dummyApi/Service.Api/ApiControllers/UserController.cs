using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.ApiControllers
{
    [ApiAuthentication]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public UserController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [ApiAuthentication]
        [HttpGet(Name = "GetCurrentUser")]
        public async Task<UserModel?> GetCurrentUser()
        {
            return await _currentUserService.GetCurrentUser();
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<List<UserModel>> GetUsers([FromQuery] bool includeBlog)
        {
            return await _userService.GetUsers(includeBlog);
        }

        [HttpGet(Name = "GetUserById")]
        public async Task<UserModel?> GetUserById([FromQuery] int id, bool includeBlog)
        {
            return await _userService.GetUserById(id, includeBlog);
        }

        [HttpPut(Name = "UpdateUser")]
        public async Task<UserModel?> UpdateUser([FromBody] UserModel userModel)
        {
          return await _userService.UpdateUser(userModel);
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task DeleteUser(int id)
        {
           await _userService.DeleteUser(id);
        }
    }
}
