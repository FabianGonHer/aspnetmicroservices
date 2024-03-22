using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authoService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authoService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto login = new();
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var response = await _authoService.LoginAsync(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                LoginResponseDto? loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(response.Result!.ToString()!);

                await SignInUserAsync(loginResponse!);

                _tokenProvider.SetToken(loginResponse!.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError("error", response!.Message);
                TempData["error"] = response!.Message;
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            RoleList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            var result = await _authoService.RegisterAsync(registrationRequestDto);

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrWhiteSpace(registrationRequestDto.RoleName))
                    registrationRequestDto.RoleName = StaticDetails.RoleCustomer;

                var roleResponse = await _authoService.AssignRoleAsync(registrationRequestDto);

                if (roleResponse != null && roleResponse.IsSuccess)
                {
                    TempData["success"] = "Registration Success";
                    RedirectToAction(nameof(Login));
                }
            }

            // Error
            TempData["error"] = result!.Message;

            RoleList();

            return View(registrationRequestDto);
        }

        private void RoleList()
        {
            ViewBag.RoleList = new List<SelectListItem>
            {
                new SelectListItem { Text = StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin },
                new SelectListItem { Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer }
            };
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        private async Task SignInUserAsync(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);
            var claims = jwt.Claims.Where(c =>
                        c.Type == JwtRegisteredClaimNames.Email ||
                        c.Type == JwtRegisteredClaimNames.Sub ||
                        c.Type == JwtRegisteredClaimNames.Name).ToList();

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(claims);
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(t => t.Type.Equals("role"))!.Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}

