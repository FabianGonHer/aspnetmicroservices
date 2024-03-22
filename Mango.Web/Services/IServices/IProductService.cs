using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService
	{
        public Task<ResponseDto?> GetAllAsync();
        public Task<ResponseDto?> GetByIdAsync(int productId);
        public Task<ResponseDto?> GetByNameAsync(string productName);
        public Task<ResponseDto?> GetByCategoryAsync(string categoryName);
        public Task<ResponseDto?> CreateAsync(ProductDto productDto);
        public Task<ResponseDto?> UpdateAsync(ProductDto productDto);
        public Task<ResponseDto?> DeleteAsync(int productId);
        public Task<ResponseDto?> GetCategoriesAsync();
    }
}

