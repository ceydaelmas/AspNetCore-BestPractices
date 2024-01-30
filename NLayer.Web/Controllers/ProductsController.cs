using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var customResponse = await _productService.GetProductsWithCategory();
            return View(customResponse.Data);//productsları dönüyorum. direkt veri lazım bize ekranda. Aslında burda service katmaında custom response dönmeme gerek yok ama böyle de olur.
        }

        public async Task<IActionResult> Save()
        {
            //List'in IEnumerable'a karşın farkı, data ile ilgili işlemler yapılabiliyor olmasıdır.
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            //dropdown için viewBag kullancam. Id bize gelen value Name ise kullancılara gözükecek valuesu.
            ViewBag.categories = new SelectList(categoryDto, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO prodctDto)
        {

            if (ModelState.IsValid)
            { //eğer her gelen değer geçeriliyse.
                await _productService.AddAsync(_mapper.Map<Product>(prodctDto));
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            //dropdown için viewBag kullancam. Id bize gelen value Name ise kullancılara gözükecek valuesu.
            ViewBag.categories = new SelectList(categoryDto, "Id", "Name");
            return View();
        }
    }
}
