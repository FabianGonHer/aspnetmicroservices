using System;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
	public class AuthService : IAuthService
	{
        private readonly IBaseService _baseService;

		public AuthService(IBaseService baseService)
		{
            _baseService = baseService;
		}

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.POST,
                Data = registrationRequestDto,
                Url = $"{Utilities.StaticDetails.AuthAPIBase}/api/auth/assingRole"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.POST,
                Data = loginRequestDto,
                Url = $"{Utilities.StaticDetails.AuthAPIBase}/api/auth/login"
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.POST,
                Data = registrationRequestDto,
                Url = $"{Utilities.StaticDetails.AuthAPIBase}/api/auth/register"
            }, withBearer: false);
        }
    }
}

