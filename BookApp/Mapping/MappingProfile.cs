using AutoMapper;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.DTOs;

namespace BookApp.Mapping
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

            CreateMap<Book, BookDTO>();
            CreateMap<UploadBookDTO, Book>()
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore()); // Ignore CoverImage

            CreateMap<BookDTO, UploadBookDTO>()
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore()); // Ignore CoverImage

            //   CreateMap<Book, BookDetailsDTO>()
            //.ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
            //.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
         
        }
    }
}