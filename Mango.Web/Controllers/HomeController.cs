using System.Diagnostics;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IProductService productService, ILogger<HomeController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Load Products in Main Page
    /// </summary>
    /// <returns>List of Products</returns>
    public async Task<IActionResult> Index()
    {
        var products = new List<ProductDto>();

        var response = await _productService.GetAllAsync();

        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!);
        }
        else
        {
            TempData["error"] = response?.Message ?? "Internal Server Error";
        }

        return View(products);
    }

    /// <summary>
    /// Get Product by Id
    /// </summary>
    /// <returns>Product Information</returns>
    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        var response = await _productService.GetByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
            return View(product);
        }

        TempData["error"] = response?.Message ?? "Internal Server Error";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

