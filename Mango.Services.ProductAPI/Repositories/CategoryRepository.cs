using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Services.ProductAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
	{
        private AppDbContext _db;
        private IMapper _mapper;
        private ResponseDto _response;

		public CategoryRepository(AppDbContext appDbContext, IMapper mapper)
		{
            _db = appDbContext;
            _mapper = mapper;
            _response = new();
		}

        public async Task<ResponseDto> Create(CategoryDto categoryDto)
        {
            try
            {
                var data = _mapper.Map<Category>(categoryDto);
                _db.Categories.Add(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CategoryDto>(data);
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

        public async Task<ResponseDto> Delete(int categoryId)
        {
            try
            {
                var item = _db.Categories.First(c => c.CategoryId == categoryId);
                _db.Categories.Remove(item);
                await _db.SaveChangesAsync();

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

        public async Task<ResponseDto> GetAll()
        {
            try
            {
                var categories = await _db.Categories.ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
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

        public async Task<ResponseDto> GetById(int categoryId)
        {
            try
            {
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                _response.Result = _mapper.Map<CategoryDto>(category);
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

        public async Task<ResponseDto> GetByName(string categoryName)
        {
            try
            {
                var categories = await _db.Categories.Select(c => c.CategoryName!.Contains(categoryName)).ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
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

        public async Task<ResponseDto> Update(CategoryDto categoryDto)
        {
            try
            {
                var data = _mapper.Map<Category>(categoryDto);
                _db.Categories.Update(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<CategoryDto>(data);
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
    }
}

