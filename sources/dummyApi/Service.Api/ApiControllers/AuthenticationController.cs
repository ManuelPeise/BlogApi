using Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.ApiControllers
{
    public class AuthenticationController:ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
    
        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost(Name = "SignIn")]
        public async Task<Response<string>> SignIn(SignInRequest signInRequest)
        {
            var token = await _authenticationService.SignInAsync(signInRequest);

            return new Response<string>
            {
                Success = token != null,
                Data = token
            };
        }

        [HttpPost(Name = "SignUp")]
        public async Task<ResponseBase> SignUp([FromBody] SignupModel signupModel)
        {
            var result = await _userService.CreateUser(signupModel);

            return new ResponseBase
            {
                Success = result,
            };
        }

        [HttpPost(Name = "ChangePassword")]
        public async Task<Response<string>> ChangePassword([FromBody] ChangePasswordRequestModel model)
        {
            var result = await _authenticationService.ChangePassword(model);

            return new Response<string>
            {
                Success = !string.IsNullOrEmpty(result),
                Data = result
            };
        }
    }
}
