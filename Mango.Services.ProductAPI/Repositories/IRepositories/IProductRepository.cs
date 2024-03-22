using Mango.Services.ProductAPI.Models.DTOs;

namespace Mango.Services.ProductAPI.Repositories.IRepositories
{
    public interface IProductRepository
	{
        public Task<ResponseDto> GetAll();
        public Task<ResponseDto> GetById(int couponId);
        public Task<ResponseDto> GetByName(string productName);
        public Task<ResponseDto> GetByCategoryId(int categoryId);
        public Task<ResponseDto> Create(ProductDto productDto);
        public Task<ResponseDto> Update(ProductDto productDto);
        public Task<ResponseDto> Delete(int productId);
    }
}

