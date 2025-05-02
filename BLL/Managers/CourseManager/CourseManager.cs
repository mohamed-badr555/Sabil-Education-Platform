using AutoMapper;
using BLL.DTOs;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Managers.CourseManager
{
    public class CourseManager : ICourseManager
    {
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IGenericRepository<Category> categoryRepo;
        private readonly IMapper mapper;

        public CourseManager(IGenericRepository<Course> courseRepo, IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _courseRepo = courseRepo;
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }


        public async Task<Pagination<CourseListDTO>> GetAllAsync(CourseSpecsParams courseparams)
        {
            var spec = new CourseSpecifications(courseparams);

            var courses = await _courseRepo.GetAllWithSpecAsync(spec);

            var Data = mapper.Map<IEnumerable<Course>, IEnumerable<CourseListDTO>>(courses);

            var countSpec = new CourseWithFiltersForCountSpecification(courseparams);

            var Count = await _courseRepo.GetCountAsync(countSpec);

            return new Pagination<CourseListDTO>(courseparams.PageIndex, courseparams.PageSize, Count, Data);
        }

        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync()
        {
            var Categories = await categoryRepo.GetAllAsync();
            var CategoriesDtos = mapper.Map<IEnumerable<CategoryReadDTO>>(Categories); // Auto-map

            return CategoriesDtos;

        }

        public async Task<Pagination<CourseListDTO>> GetPopularAsync()
        {
            var spec = new PopularCourseSpecification();
            var courses = await _courseRepo.GetAllWithSpecAsync(spec);

            var Data = mapper.Map<IEnumerable<Course>, IEnumerable<CourseListDTO>>(courses);

            return new Pagination<CourseListDTO>(1, 5 , 5, Data);
        }



       

        public async Task<CourseDetailsDTO> GetByIdAsync(string id)
        {
            var CourseModel = await _courseRepo.GetByIdAsync(id);
            if (CourseModel == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            var CourseDto = mapper.Map<CourseDetailsDTO>(CourseModel);
            return CourseDto;
        }

        public async Task AddAsync(CourseAddDTO course_dto)
        {
            
            var course = mapper.Map<Course>(course_dto);
            await _courseRepo.InsertAsync(course);
        }

        public async Task<List<CourseListDTO>> SearchCoursesAsync(string searchTerm)
        {
            var courses = await _courseRepo.GetAllAsync();
            var courses_dtos = mapper.Map<List<CourseListDTO>>(courses);
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                courses_dtos = courses_dtos
                    .Where(c => c.Title.ToLower().Contains(searchTerm) ||
                                c.Description.ToLower().Contains(searchTerm))
                    .ToList();
            }

            return courses_dtos;
        }


        public async Task<List<CourseListDTO>> FilterCoursesAsync(string level, string category)
        {
            var courses = await _courseRepo.GetAllAsync();
            var courseDTOs = mapper.Map<List<CourseListDTO>>(courses);

            if (!string.IsNullOrWhiteSpace(level))
                level = level.ToLower();
            if (!string.IsNullOrWhiteSpace(category))
                category = category.ToLower();

            courseDTOs = courseDTOs.Where(c =>
                (string.IsNullOrWhiteSpace(level) || c.Level.ToLower().Contains(level)) &&
                (string.IsNullOrWhiteSpace(category) || c.CategoryName.ToLower().Contains(category))
            ).ToList();

            return courseDTOs;
        }



        public async Task UpdateAsync(CourseAddDTO CourseDTO)
        {

   
            var course = await _courseRepo.GetByIdAsync(CourseDTO.Id);
      
            if (course == null)
                throw new Exception("Course not found");

            mapper.Map(CourseDTO, course); // maps *into* the existing object


            await _courseRepo.UpdateAsync(course);
        }


        public async Task DeleteAsync(string id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            await _courseRepo.DeleteAsync(course);
        }

       
    }

    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }

        public Pagination(int pageIndex, int pageSize, int count, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

    }
}
