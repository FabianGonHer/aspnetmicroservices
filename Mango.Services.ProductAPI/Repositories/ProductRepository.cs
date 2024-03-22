using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Services.ProductAPI.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
	{
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public ProductRepository(AppDbContext appDbContext, IMapper mapper)
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
                //var products = await _db.Connection.GetAllAsync<Product>();

                // Get data by EF
                var products = await _db.Products.Include(p => p.Category).ToListAsync();

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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

        public async Task<ResponseDto> GetById(int productId)
        {
            try
            {
                // Get data by Dapper
                //var products = await _db.Connection.GetAllAsync<Product>();

                // Get data by EF
                var product = await _db.Products.Include(c => c.Category)
                    .FirstAsync(c => c.ProductId == productId);

                _response.Result = _mapper.Map<ProductDto>(product);
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

        public async Task<ResponseDto> GetByName(string productName)
        {
            try
            {
                // Get data by Dapper
                //var product = await _db.Connection.GetAsync<Product>(new() { ProductName = productName.ToLower()});

                // Get data by EF
                var product = await _db.Products.Include(c => c.Category)
                    .FirstAsync(p => p.Name!.ToLower().Equals(productName.ToLower()));

                _response.Result = _mapper.Map<ProductDto>(product);
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

        public async Task<ResponseDto> GetByCategoryId(int categoryId)
        {
            try
            {
               // Get data by EF
                var products = await _db.Products.Select(p => p.CategoryId == categoryId).ToListAsync();

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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

        public async Task<ResponseDto> Create(ProductDto productDto)
        {
            try
            {
                var data = _mapper.Map<Product>(productDto);
                _db.Products.Add(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDto>(data);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> Update(ProductDto productDto)
        {
            try
            {
                var data = _mapper.Map<Product>(productDto);
                _db.Products.Update(data);
                await _db.SaveChangesAsync();

                _response.Result = _mapper.Map<ProductDto>(data);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }

            return _response;
        }

        public async Task<ResponseDto> Delete(int productId)
        {
            try
            {
                var item = _db.Products.First(c => c.ProductId == productId);
                _db.Products.Remove(item);
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

