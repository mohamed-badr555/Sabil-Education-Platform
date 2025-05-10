using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using BLL.DTOs.Basket;
using BLL.DTOs.CategoryManagement;
using DAL.Data.Models;
using DAL.Data.Models.Basket;

namespace BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Fix the mapping for Course to CourseDetailsDTO
            CreateMap<Course, CourseDetailsDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => GetCourseTypeString(src.CourseType)));

            CreateMap<CourseDetailsDTO, Course>()
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => GetCourseTypeInt(src.CourseType)));

            // Fix the mapping for Course to CourseListDTO - focus on the Id property
            CreateMap<Course, CourseListDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Explicitly map string Id to int Id
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<CourseListDTO, Course>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())); // Convert int Id back to string

            // Fix mapping for CourseAddDTO to Course
            CreateMap<Course, CourseAddDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => GetCourseTypeString(src.CourseType)));

            CreateMap<CourseAddDTO, Course>()
                .ForMember(dest => dest.CourseType, opt => opt.MapFrom(src => GetCourseTypeInt(src.CourseType)))
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));

            CreateMap<Category, CategoryReadDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.Name));

            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));


            CreateMap<Course, CourseContentDTO>()
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.CourseAccounts.Count))
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.CourseUnits));

            // Fix for CourseUnit to UnitDTO mapping
            CreateMap<CourseUnit, UnitDTO>()
                .ForMember(dest => dest.videos, opt => opt.MapFrom(src => src.videos));

            CreateMap<UnitDTO, CourseUnit>()
                .ForMember(dest => dest.videos, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));

            // Add Video to VideoDetailsDTO mapping
            CreateMap<Video, VideoDetailsDTO>()
                .ForMember(dest => dest.VideoComments, opt => opt.MapFrom(src => src.VideoComments));

            CreateMap<VideoDetailsDTO, Video>()
                .ForMember(dest => dest.VideoComments, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)));

            // Add VideoComment to VideoComment mapping (if needed)
            CreateMap<VideoComment, VideoComment>();
        }

        // Helper methods to convert between string and int for CourseType
        private string GetCourseTypeString(int courseType)
        {
            return courseType switch
            {
                1 => "Online",
                2 => "Recorded",
                3 => "Live",
                _ => "Online", // Default
            };
        }

        private int GetCourseTypeInt(string courseType)
        {
            return courseType?.ToLower() switch
            {
                "online" => 1,
                "recorded" => 2,
                "live" => 3,
                _ => 1, // Default to Online
            };
        }
    }
}
