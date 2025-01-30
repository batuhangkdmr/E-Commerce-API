using AutoMapper;
using ECommerceAPI.Application.DTOs.Categories;
using ECommerceAPI.Application.DTOs.Prodcuts;
using ECommerceAPI.Application.DTOs.Users;
using ECommerceAPI.Application.Features.Commands.AppUsers.CreateUser;
using ECommerceAPI.Application.Features.Commands.AppUsers.UpdateUser;
using ECommerceAPI.Application.Features.Commands.Categories.CreateCategory;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Entities.Identity;

namespace ECommerceAPI.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Create User Mappings
            CreateMap<AppUser, CreateUserCommandRequest>()
                .ReverseMap();
            CreateMap<CreateUserDTO, CreateUserCommandRequest>()
                .ReverseMap();
            CreateMap<CreateUserResponseDTO, CreateUserCommandResponse>()
                .ReverseMap();

            // Product and Category Mappings
            CreateMap<CreateProductDTO, Product>()
                .ReverseMap();
            CreateMap<Category, CreateCategoryCommandRequest>()
                .ReverseMap();
            CreateMap<Category, GetRootCategoriesResponseDTO>()
                .ReverseMap();
            CreateMap<Category, GetSubCategoriesResponseDTO>()
                .ReverseMap();

            // Update User Mappings
            CreateMap<UpdateUserCommandRequest, UpdatedUserDTO>() // Güncelleme isteğini DTO'ya dönüştürme
                .ReverseMap();
            CreateMap<UpdatedUserDTO, UpdateUserCommandResponse>() // Güncellenmiş kullanıcı DTO'sunu komut yanıtına dönüştürme
                .ForMember(dest => dest.Succeeded, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "User updated successfully"))
                .ForMember(dest => dest.UpdatedUser, opt => opt.MapFrom(src => src));
        }
    }

}
