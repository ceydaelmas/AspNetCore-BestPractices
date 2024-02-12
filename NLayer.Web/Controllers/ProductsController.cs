using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Filters;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            var customResponse = await _productApiService.GetProductsWithCategoryAsync();
            return View(customResponse);//productsları dönüyorum. direkt veri lazım bize ekranda. Aslında burda service katmaında custom response dönmeme gerek yok ama böyle de olur.
        }

        public async Task<IActionResult> Save()
        {
            //List'in IEnumerable'a karşın farkı, data ile ilgili işlemler yapılabiliyor olmasıdır.
            var categories = await _categoryApiService.GetAllAsync();
            //dropdown için viewBag kullancam. Id bize gelen value Name ise kullancılara gözükecek valuesu.
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDto)
        {

            if (ModelState.IsValid)
            { //eğer her gelen değer geçeriliyse.
                await _productApiService.SaveAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAllAsync();
            //dropdown için viewBag kullancam. Id bize gelen value Name ise kullancılara gözükecek valuesu.
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        //filterim constructorunda parametre aldığı için servideFilter ile tanımlıyorum.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update (int id)
        {
            //ilk sayfa yüklendiğinde bu method çalışıyor. Ben zaten burdan productı dönüyorum UI'da içleri dolu olacak.
            var product = await _productApiService.GetByIdAsync(id);
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]//web tarafında put kullanılmıyor.
        public async Task<IActionResult> Update(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            //eğer modelstate valid değilse yani boş olan değerler varsa categroy dropdownın içi dolsun diye aşağıdakini yapıyoruz.çünkü sayfa yüklendiğindeki method tekrar çalışmayacak. tekrar doldurmam gerek.
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name",productDto.CategoryId);
            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
