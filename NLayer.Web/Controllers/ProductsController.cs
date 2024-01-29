using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var customResponse = await _productService.GetProductsWithCategory();
            return View(customResponse.Data);//productsları dönüyorum. direkt veri lazım bize ekranda. Aslında burda service katmaında custom response dönmeme gerek yok ama böyle de olur.
        }
    }
}
