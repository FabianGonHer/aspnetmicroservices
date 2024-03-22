using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        /// <summary>
        /// Get all coupons from the service.
        /// </summary>
        /// <returns>View object with a list of coupons.</returns>
        public async Task<IActionResult> CouponIndex()
        {
            var coupons = new List<CouponDto>();

            var response = await _couponService.GetAllAsync();

            if (response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result)!);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(coupons);
        }

        // Button Action to Load Cupon Create View.
        public IActionResult CouponCreate()
        {
            return View();
        }

        /// <summary>
        /// Create new coupon
        /// </summary>
        /// <param name="couponDto"></param>
        /// <returns>If coupon created, redirect to List of Coupons View</returns>
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateAsync(couponDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon Created Successfuly";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(couponDto);
        }

        // Button Action to Load Coupon Delete View.
        public IActionResult CouponDelete(int couponId)
        {
            var response = _couponService.GetByIdAsync(couponId).Result;
            
            if (response != null && response.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result)!);
                return View(coupon);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View();
        }

        /// <summary>
        /// Delete specific coupon by ID
        /// </summary>
        /// <param name="couponDto"></param>
        /// <returns>Redirect to Coupon List View</returns>
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            var response = await _couponService.DeleteAsync(couponDto.Id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted Successfuly";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(couponDto);
        }
    }
}

