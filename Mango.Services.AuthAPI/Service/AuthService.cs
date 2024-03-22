using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTOs;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _appDbContext = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.Email!.ToLower() == email.ToLower());

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // Create Role
                    _roleManager.CreateAsync(new IdentityRole { Name = roleName }).GetAwaiter().GetResult();
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }

            return user != null;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.UserName!.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, loginRequestDto.Password);

            if (user is null || !isValid)
            {
                return new LoginResponseDto { Token = string.Empty };
            }

            var roles = await _userManager.GetRolesAsync(user);

            //TODO: Handle Exception when roles == 0

            var jwtToken = _jwtTokenGenerator.GeneratorToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email!,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber!
            };

            return new LoginResponseDto { User = userDto, Token = jwtToken };
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                Email = registrationRequestDto.Email,
                Name = registrationRequestDto.Name,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                UserName = registrationRequestDto.Email,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            var message = string.Empty;

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (!result.Succeeded)
            {
                message = result.Errors.FirstOrDefault()!.Description;
            }

            return message;
        }
    }
}

