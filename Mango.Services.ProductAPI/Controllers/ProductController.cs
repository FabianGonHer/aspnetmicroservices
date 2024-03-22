using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Services.ProductAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            var response = await _productRepository.GetAll();
            return Ok(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> Get(int id)
        {
            var response = await _productRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public async Task<ActionResult<ResponseDto>> GetByName(string name)
        {
            var response = await _productRepository.GetByName(name);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetByCategory/{category}")]
        public async Task<ActionResult<ResponseDto>> GetByCategoryId(int categoryId)
        {
            var response = await _productRepository.GetByCategoryId(categoryId);
            return Ok(response);
        }

        // POST api/values
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Post([FromBody] ProductDto productDto)
        {
            var response = await _productRepository.Create(productDto);
            return Ok(response);
        }

        // PUT api/values/5
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Put(int productId, [FromBody] ProductDto productDto)
        {
            if (productId != productDto.ProductId)
            {
                return BadRequest(new ResponseDto { Message = "Missmatching Product ID." });
            }

            var response = await _productRepository.Update(productDto);
            return Ok(response);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            var response = await _productRepository.Delete(id);
            return Ok(response);
        }

        #region Categories

        [HttpGet("categories")]
        public async Task<ActionResult<ResponseDto>> GetCategories()
        {
            var response = await _categoryRepository.GetAll();
            return Ok(response);
        }

        #endregion
    }
}

