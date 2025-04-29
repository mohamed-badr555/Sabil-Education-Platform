using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTOs;
using DAL.Data.Models;

namespace BLL.MappingProfiles
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<Course, CourseDetailsDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CourseDetailsDTO, Course>();

            CreateMap<Course, CourseListDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CourseListDTO, Course>();

            CreateMap<Course, CourseAddDTO>();
            CreateMap<CourseAddDTO, Course>();

            CreateMap<Category,CategoryReadDTO>();
        }
    }
}
