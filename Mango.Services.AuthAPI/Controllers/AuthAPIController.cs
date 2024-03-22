using Mango.Services.AuthAPI.Models.DTOs;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto request)
        {
            var message = await _authService.Register(request);

            if (!string.IsNullOrEmpty(message))
            {
                _responseDto.Message = message;
                _responseDto.IsSuccess = false;

                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = await _authService.Login(loginRequestDto);

            if (response.User is null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or Password is incorrect";

                return BadRequest(_responseDto);
            }

            _responseDto.Result = response;
            return Ok(_responseDto);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto roleRequestDto)
        {
            var response = await _authService.AssignRole(roleRequestDto.Email, roleRequestDto.RoleName);

            if (!response)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error assigning role.";

                return BadRequest(_responseDto);
            }
            
            return Ok(_responseDto);
        }
    }
}

