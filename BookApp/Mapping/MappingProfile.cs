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

            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<UploadBookDTO, Book>()
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore()); // Ignore CoverImage

            CreateMap<BookDTO, UploadBookDTO>()
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore()); // Ignore CoverImage

          CreateMap<Book, BookDetailsDTO>()
         .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.FullName))
         .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name));

            CreateMap<BookDetailsDTO, UploadBookDTO>().ReverseMap();
            CreateMap<Book, UploadBookDTO>().ForMember(dest => dest.CoverImage, opt => opt.Ignore()); // Ignore CoverImage

            CreateMap<AppUser, AppUserDTO>().ReverseMap();

            // New mappings for Sold and related DTOs
            CreateMap<Sold, SoldCreateDTO>().ReverseMap();
              

            CreateMap<SoldCreateViewModel, Sold>()
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.BookId, opt => opt.Ignore());

            CreateMap<Book, BookQuantityDTO>()
                .ForMember(dest => dest.Quantity, opt => opt.Ignore());

            // Reverse mappings
            CreateMap<SoldCreateDTO, Sold>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<SoldDTO, Sold>().ReverseMap();
            CreateMap<UserCartDto, UserCart>().ReverseMap();


        }
    }
}