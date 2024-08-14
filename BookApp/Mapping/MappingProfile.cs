using AutoMapper;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Shared.DTOs;
using System.Reflection.Metadata;

namespace Safary.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, RegisterDTO>().ReverseMap();
            CreateMap<RegisterDTO, AppUser>().ReverseMap();
            CreateMap<AppUser, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.FirstName));

            CreateMap<CategoryDTO, UploadCategoryDTO>().ReverseMap();

            CreateMap<CategoryDTO, Category>().ReverseMap();

            CreateMap<AuthorDTO, UploadAuthorDTO>().ReverseMap();

            CreateMap<AuthorDTO, Author>().ReverseMap();


        }
    }
}