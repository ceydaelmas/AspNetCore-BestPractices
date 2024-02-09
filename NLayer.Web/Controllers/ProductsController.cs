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
        public async Task<IActionResult> Save(ProductDTO productDto)
        {

            if (ModelState.IsValid)
            { //eğer her gelen değer geçeriliyse.
                await _productService.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            //dropdown için viewBag kullancam. Id bize gelen value Name ise kullancılara gözükecek valuesu.
            ViewBag.categories = new SelectList(categoryDto, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update (int id)
        {
            //ilk sayfa yüklendiğinde bu method çalışıyor. Ben zaten burdan productı dönüyorum UI'da içleri dolu olacak.
            var product = await _productService.GetByIdAsync(id);
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.categories = new SelectList(categoryDto, "Id", "Name", product.CategoryId);
            return View(_mapper.Map<ProductDTO>(product));
        }

        [HttpPost]//web tarafında put kullanılmıyor.
        public async Task<IActionResult> Update(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }
            //eğer modelstate valid değilse yani boş olan değerler varsa categroy dropdownın içi dolsun diye aşağıdakini yapıyoruz.çünkü sayfa yüklendiğindeki method tekrar çalışmayacak. tekrar doldurmam gerek.
            var categories = await _categoryService.GetAllAsync();
            var categoryDto = _mapper.Map<List<CategoryDTO>>(categories.ToList());
            ViewBag.categories = new SelectList(categoryDto, "Id", "Name",productDto.CategoryId);
            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            await _productService.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
