using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
	{
		public Task<ResponseDto?> GetAllAsync();
		public Task<ResponseDto?> GetByIdAsync(int couponId);
		public Task<ResponseDto?> GetByCodeAsync(string couponCode);
		public Task<ResponseDto?> CreateAsync(CouponDto couponDto);
		public Task<ResponseDto?> UpdateAsync(CouponDto couponDto);
		public Task<ResponseDto?> DeleteAsync(int couponId);
	}
}

