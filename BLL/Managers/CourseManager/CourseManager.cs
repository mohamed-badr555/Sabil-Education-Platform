using AutoMapper;
using BLL.DTOs;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CourseManager> _logger;

        public CourseManager(IGenericRepository<Course> courseRepo, IGenericRepository<Category> categoryRepo, IMapper mapper
            , ILogger<CourseManager> logger)
        {
            _courseRepo = courseRepo;   
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
            _logger = logger;
        }

        public async Task<Pagination<CourseListDTO>> GetAllAsync(CourseSpecsParams courseparams)
        {
            try
            {
                var spec = new CourseSpecifications(courseparams);
                var courses = await _courseRepo.GetAllWithSpecAsync(spec);

                // Use explicit mapping with error handling
                var data = courses.Select(course =>
                {
                    try
                    {
                        return mapper.Map<CourseListDTO>(course);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error mapping Course {Id} to CourseListDTO", course.Id);
                        return null;
                    }
                }).Where(dto => dto != null).ToList();

                var countSpec = new CourseWithFiltersForCountSpecification(courseparams);
                var count = await _courseRepo.GetCountAsync(countSpec);

                return new Pagination<CourseListDTO>(
                    courseparams.PageIndex,
                    courseparams.PageSize,
                    count,
                    data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                throw;
            }
        }

        public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync()
        {
            var Categories = await categoryRepo.GetAllAsync();
            var CategoriesDtos = mapper.Map<IEnumerable<CategoryReadDTO>>(Categories);

            return CategoriesDtos;
        }

        public async Task<Pagination<CourseListDTO>> GetPopularAsync()
        {
            var spec = new PopularCourseSpecification();
            var courses = await _courseRepo.GetAllWithSpecAsync(spec);

            var Data = mapper.Map<IEnumerable<Course>, IEnumerable<CourseListDTO>>(courses);

            return new Pagination<CourseListDTO>(1, 5, 5, Data);
        }

        public async Task<CourseDetailsDTO> GetByIdAsync(string id)
        {
            var CourseModel = await _courseRepo.GetByIdAsync(id);
            if (CourseModel == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            var CourseDto = mapper.Map<CourseDetailsDTO>(CourseModel);
            return CourseDto;
        }

        public async Task<CourseAddDTO> AddAsync(CourseAddDTO courseDto)
        {
            try
            {
                // Generate a new ID for the course if one is not provided
                if (string.IsNullOrEmpty(courseDto.Id))
                {
                    courseDto.Id = Guid.NewGuid().ToString();
                }

                // Set default values if missing
                courseDto.Level = courseDto.Level ?? "Beginner";
                courseDto.Last_Update = DateTime.Now;

                // Map the DTO to entity and insert
                var course = mapper.Map<Course>(courseDto);
                _logger.LogInformation($"Mapping CourseAddDTO to Course: Title={courseDto.Title}, CategoryId={courseDto.CategoryId}, CourseType={courseDto.CourseType}, CourseTypeInt={course.CourseType}");

                await _courseRepo.InsertAsync(course);

                // Map back to DTO and return
                return mapper.Map<CourseAddDTO>(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding course");
                throw;
            }
        }

        //public async Task<List<CourseListDTO>> SearchCoursesAsync(string searchTerm)
        //{
        //    var courses = await _courseRepo.GetAllAsync();
        //    var coursesDtos = mapper.Map<List<CourseListDTO>>(courses);
            
        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        searchTerm = searchTerm.ToLower();

        //        coursesDtos = coursesDtos
        //            .Where(c => c.Title.ToLower().Contains(searchTerm) ||
        //                        c.Description.ToLower().Contains(searchTerm))
        //            .ToList();
        //    }

        //    return coursesDtos;
        //}

        //public async Task<List<CourseListDTO>> FilterCoursesAsync(string level, string category)
        //{
        //    var courses = await _courseRepo.GetAllAsync();
        //    var courseDTOs = mapper.Map<List<CourseListDTO>>(courses);

        //    if (!string.IsNullOrWhiteSpace(level))
        //        level = level.ToLower();
        //    if (!string.IsNullOrWhiteSpace(category))
        //        category = category.ToLower();

        //    courseDTOs = courseDTOs.Where(c =>
        //        (string.IsNullOrWhiteSpace(level) || c.Level.ToLower().Contains(level)) &&
        //        (string.IsNullOrWhiteSpace(category) || c.CategoryName.ToLower().Contains(category))
        //    ).ToList();

        //    return courseDTOs;
        //}

        public async Task UpdateAsync(CourseAddDTO courseDto)
        {
            var course = await _courseRepo.GetByIdAsync(courseDto.Id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseDto.Id} not found.");

            // Ensure we're not overwriting the ThumbnailUrl with null
            if (string.IsNullOrEmpty(courseDto.ThumbnailUrl))
            {
                courseDto.ThumbnailUrl = course.ThumbnailUrl;
            }

            mapper.Map(courseDto, course);
            await _courseRepo.UpdateAsync(course);
        }

        public async Task DeleteAsync(string id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            // Use SoftDeleteAsync instead of DeleteAsync for soft deletion
            await _courseRepo.SoftDeleteAsync(course);
        }

        //public async Task<bool> ExistsAsync(string id)
        //{
        //    var course = await _courseRepo.GetByIdAsync(id);
        //    return course != null;
        //}

        //public async Task<bool> ExistsByTitleAsync(string title)
        //{
        //    return await _courseRepo.AnyAsync(c => c.Title == title && !c.IsDeleted);
        //}

        //public async Task<int> GetCoursesCountAsync()
        //{
        //    var courses = await _courseRepo.GetAllAsync();
        //    return courses.Count;
        //}
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
