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
            //CreateMap<Course, CourseContentDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            //CreateMap<CourseContentDTO, Course>();

            CreateMap<Course, CourseListDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CourseListDTO, Course>();

            CreateMap<Course, CourseAddDTO>();
            CreateMap<CourseAddDTO, Course>();

            CreateMap<Category,CategoryReadDTO>();

            CreateMap<Video, VideoDetailsDTO>()
       .ForMember(dest => dest.unitorder,
                  opt => opt.MapFrom(src => src.CourseUnit.Order));


            CreateMap<CourseUnit, UnitDTO>();
            CreateMap<Course, CourseContentDTO>()
            .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => src.CourseAccounts.Count))
            .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.CourseUnits));
        }


    }
}
