using System.Dynamic;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products from the service.
        /// </summary>
        /// <returns>View object with a list of products.</returns>
        public async Task<IActionResult> ProductIndex()
        {
            var products = new List<ProductDto>();

            var response = await _productService.GetAllAsync();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(products);
        }

        // Button Action to Load Product Create View.
        public async Task<IActionResult> ProductCreate()
        {
            await GetCategoriesAsync();
            return View();
        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns>If product created, redirect to List of Products View</returns>
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product Created Successfuly";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            await GetCategoriesAsync();

            return View(productDto);
        }

        //Button Action to Load Product Edit View
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                await GetCategoriesAsync();
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
                return View(product);
            }

            TempData["error"] = response?.Message ?? "Internal Server Error";

            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateAsync(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product Updated Successfuly";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            await GetCategoriesAsync();

            return View(productDto);
        }

        // Button Action to Load Product Delete View.
        public IActionResult ProductDelete(int productId)
        {
            var response = _productService.GetByIdAsync(productId).Result;

            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View();
        }

        /// <summary>
        /// Delete specific product by ID
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns>Redirect to Product List View</returns>
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            var response = await _productService.DeleteAsync(productDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Deleted Successfuly";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        private async Task GetCategoriesAsync()
        {
            var response = await _productService.GetCategoriesAsync();

            if (response != null && response.IsSuccess)
            {
                var categories = JsonConvert.DeserializeObject<List<CategoryDto>>(Convert.ToString(response.Result)!)!;
                ViewBag.Categories = categories;
            }
            else
            {
                TempData["error"] = response?.Message;
            }
        }
    }
}

