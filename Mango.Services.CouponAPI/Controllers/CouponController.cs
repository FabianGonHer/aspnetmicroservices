using Mango.Services.CouponAPI.Models.DTOs;
using Mango.Services.CouponAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CouponController : Controller
    {
        private ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        // GET: api/values
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            var response = await _couponRepository.GetAll();
            return Ok(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto>> Get(int id)
        {
            var response = await _couponRepository.GetById(id);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDto>> GetByCode(string code)
        {
            var response = await _couponRepository.GetByCode(code);
            return Ok(response);
        }

        // POST api/values
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Post([FromBody] CouponDto couponDto)
        {
            var response = await _couponRepository.Create(couponDto);
            return Ok(response);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Put([FromBody] CouponDto couponDto)
        {
            var response = await _couponRepository.Update(couponDto);
            return Ok(response);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            var response = await _couponRepository.Delete(id);
            return Ok(response);
        }
    }
}

