using Mango.Web.Services.IServices;
using Mango.Web.Utilities;
using Newtonsoft.Json.Linq;

namespace Mango.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Delete(StaticDetails.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;

            _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);
            
            return token;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(StaticDetails.TokenCookie, token);
        }
    }
}

