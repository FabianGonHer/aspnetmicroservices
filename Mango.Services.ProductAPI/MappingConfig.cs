using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTOs;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
	{
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>()
                    .ForMember(p => p.CategoryName, opt => opt.MapFrom(c => c.Category!.CategoryName));
                config.CreateMap<CategoryDto, Category>();
                config.CreateMap<Category, CategoryDto>();
            });

            return mappingConfig;
        }
    }
}

