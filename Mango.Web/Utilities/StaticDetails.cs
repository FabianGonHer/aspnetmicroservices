namespace Mango.Web.Utilities
{
    public class StaticDetails
	{
		public static string? CouponAPIBase { get; set; }
		public static string? AuthAPIBase { get; set; }
		public static string? ProductAPIBase { get; set; }
		public const string RoleAdmin = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
		public const string TokenCookie = "JWTToken";
		public static TimeSpan defaultCacheDuration = TimeSpan.FromMinutes(60);

		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}

