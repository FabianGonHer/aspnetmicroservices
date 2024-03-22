using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
	{
		private readonly IBaseService _baseService;

		public CouponService(IBaseService baseService)
		{
			_baseService = baseService;
		}

		public async Task<ResponseDto?> GetAllAsync()
		{
			return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.GET,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon"
            });
		}

        public async Task<ResponseDto?> GetByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.GET,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDto?> GetByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.GET,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon/{couponId}"
            });
        }

        public async Task<ResponseDto?> CreateAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.POST,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon",
                Data = couponDto
            });
        }

        public async Task<ResponseDto?> DeleteAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.DELETE,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon/{couponId}"
            });
        }

        public async Task<ResponseDto?> UpdateAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = Utilities.StaticDetails.ApiType.PUT,
                Url = $"{Utilities.StaticDetails.CouponAPIBase}/api/coupon",
                Data = couponDto
            });
        }
    }
}

