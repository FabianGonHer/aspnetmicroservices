using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOs;
using Mango.Services.CouponAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
	{
        private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private ResponseDto _response;

        public CouponRepository(AppDbContext appDbContext, IMapper mapper)
		{
			_db = appDbContext;
			_mapper = mapper;
			_response = new();
		}

		public async Task<ResponseDto> GetAll()
		{
			try
			{
                // Get data by Dapper
                //var coupons = await _db.Connection.GetAllAsync<Coupon>();

                // Get data by EF
                var coupons = await _db.Coupons.ToListAsync();

                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
				_response.IsSuccess = true;
            }
			catch(Npgsql.PostgresException npgEx)
			{
                _response.Message = npgEx.Message;
            }
			catch (Exception ex)
			{
				_response.Message = ex.Message;
			}

			return _response;
		}

		public async Task<ResponseDto> GetById(int couponId)
		{
            try
            {
                // Get data by Dapper
                //var coupons = await _db.Connection.GetAllAsync<Coupon>();

                // Get data by EF
                var coupon = await _db.Coupons.FirstAsync(c => c.Id == couponId); 

                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.IsSuccess = true;
            }
			catch(Npgsql.PostgresException npgEx)
			{
                _response.Message = npgEx.Message;
            }
			catch (Exception ex)
			{
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> GetByCode(string couponCode)
        {
            try
            {
                // Get data by Dapper
                //var coupon = await _db.Connection.GetAsync<Coupon>(new() { CouponCode = couponCode.ToLower()});

                // Get data by EF
                var coupon = await _db.Coupons.FirstAsync(c => c.CouponCode!.ToLower().Equals(couponCode.ToLower()));

                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.IsSuccess = true;
            }
            catch (Npgsql.PostgresException npgEx)
            {
                _response.Message = npgEx.Message;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }
        
        public async Task<ResponseDto> Create(CouponDto couponDto)
        {
            try
            {
                var data = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CouponDto>(data);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> Update(CouponDto couponDto)
        {
            try
            {
                var data = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CouponDto>(data);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> Delete(int couponId)
        {
            try
            {
                var item = _db.Coupons.First(c => c.Id == couponId);
                _db.Coupons.Remove(item);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
