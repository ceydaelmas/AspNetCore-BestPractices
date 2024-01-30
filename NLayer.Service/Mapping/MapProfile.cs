using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDTO>().ReverseMap();
            CreateMap<ProductUpdateDTO, Product>();//DTO'yu göründe entitye dönüştürdem ama entityi updatedtoYa dönüştürmem ondan dolayı reverse gerek yok.
            CreateMap<Product, ProductWithCategoryDTO>(); //ProductServicede productsı product with dtoya dönüştürüyoruz.
            CreateMap<Category, CategoryWithProductsDTO>();
        }
    }
}
