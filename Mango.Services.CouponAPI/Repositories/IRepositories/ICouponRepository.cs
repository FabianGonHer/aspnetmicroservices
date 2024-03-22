using Mango.Services.CouponAPI.Models.DTOs;

namespace Mango.Services.CouponAPI.Repositories.IRepositories
{
    public interface ICouponRepository
	{
		public Task<ResponseDto> GetAll();
		public Task<ResponseDto> GetById(int couponId);
        public Task<ResponseDto> GetByCode(string code);
        public Task<ResponseDto> Create(CouponDto couponDto);
        public Task<ResponseDto> Update(CouponDto couponDto);
        public Task<ResponseDto> Delete(int couponId);
    }
}

