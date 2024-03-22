using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Services.CouponAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder AddAuthenticationBuilder(this WebApplicationBuilder builder)
		{
            var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret") ?? throw new NullReferenceException("Missing Secret");
            var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer") ?? throw new NullReferenceException("Missing Issuer");
            var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience") ?? throw new NullReferenceException("Missing Audience");

            var key = Encoding.ASCII.GetBytes(secret!);

            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(t =>
            {
                t.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience
                };
            });
            builder.Services.AddAuthorization();

            return builder;
        }
	}
}

