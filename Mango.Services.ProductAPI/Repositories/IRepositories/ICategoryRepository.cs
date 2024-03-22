using Mango.Services.ProductAPI.Models.DTOs;

namespace Mango.Services.ProductAPI.Repositories.IRepositories
{
    public interface ICategoryRepository
	{
        public Task<ResponseDto> GetAll();
        public Task<ResponseDto> GetById(int categoryId);
        public Task<ResponseDto> GetByName(string categoryName);
        public Task<ResponseDto> Create(CategoryDto categoryDto);
        public Task<ResponseDto> Update(CategoryDto categoryDto);
        public Task<ResponseDto> Delete(int categoryId);
    }
}

