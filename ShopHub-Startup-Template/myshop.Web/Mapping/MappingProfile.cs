using AutoMapper;
using myshop.Entities.DTOs;
using myshop.Entities.Models;
using System.Runtime;

namespace myshop.Web.Mapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ReverseMap();

        }
    }
}
