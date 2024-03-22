using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
	{
		private readonly IBaseService _baseService;
        private readonly IMemoryCache _memoryCache;
        private readonly string _categoriesKey = "categories";

        public ProductService(IBaseService baseService, IMemoryCache memoryCache)
		{
			_baseService = baseService;
            _memoryCache = memoryCache;
		}

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product"
            });
        }

        public async Task<ResponseDto?> GetByNameAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/GetByName/{productName}"
            });
        }

        public async Task<ResponseDto?> GetByCategoryAsync(string categoryName)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/GetByCategory/{categoryName}"
            });
        }

        public async Task<ResponseDto?> GetByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/{productId}"
            });
        }

        public async Task<ResponseDto?> CreateAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.POST,
                Url = $"{StaticDetails.ProductAPIBase}/api/product",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.DELETE,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/{productId}"
            });
        }

        public async Task<ResponseDto?> UpdateAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                apiType = StaticDetails.ApiType.PUT,
                Url = $"{StaticDetails.ProductAPIBase}/api/product/{productDto.ProductId}",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> GetCategoriesAsync()
        {
            return await _memoryCache.GetOrCreateAsync(_categoriesKey, async entry =>
            {
                entry.SlidingExpiration = StaticDetails.defaultCacheDuration;
                return await _baseService.SendAsync(new RequestDto
                {
                    apiType = StaticDetails.ApiType.GET,
                    Url = $"{StaticDetails.ProductAPIBase}/api/product/categories"
                });
            });
        }
    }
}
