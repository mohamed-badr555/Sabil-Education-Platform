using AutoMapper;
using BLL.DTOs;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using DAL.Repositories;
using DAL.Repositories.CourseRepo;
using DAL.Repositories.VideoRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly ICourseRepo _courseRepo;
        private readonly IGenericRepository<Category> categoryRepo;
        private readonly IVideoRepo videoRepo;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<CourseManager> _logger;

        public CourseManager(ICourseRepo courseRepo, ILogger<CourseManager> logger, IGenericRepository<Category> categoryRepo, IVideoRepo _videoRepo, IMapper mapper , IMemoryCache memoryCache)
        {
            _courseRepo = courseRepo;   
            this.categoryRepo = categoryRepo;
            videoRepo = _videoRepo;
            this.mapper = mapper;
            _logger = logger;
            this.memoryCache = memoryCache;
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
            const string cacheKey = "popular_courses";

            // Try to get from cache first
            if (memoryCache.TryGetValue(cacheKey, out Pagination<CourseListDTO> cachedResult))
            {
                return cachedResult;
            }

           
            var spec = new PopularCourseSpecification();
            var courses = await _courseRepo.GetAllWithSpecAsync(spec);
            var data = mapper.Map<IEnumerable<Course>, IEnumerable<CourseListDTO>>(courses);

            var result = new Pagination<CourseListDTO>(1, 5, 5, data);

            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30)) // Cache for 30 minutes
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));   // Maximum cache duration

            // Save data in cache
            memoryCache.Set(cacheKey, result, cacheOptions);

            return result;
        }
        public async Task<CourseContentDTO> GetByIdAsync(string id)
        {
            var CourseModel = await _courseRepo.GetByIdAsync(id);
            if (CourseModel == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            var CourseDto = mapper.Map<CourseContentDTO>(CourseModel);
            return CourseDto;
        }

        public async Task AddAsync(CourseAddDTO courseDto)
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
                //return mapper.Map<CourseAddDTO>(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding course");
                throw;
            }
        }





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
            // Invalidate the cache
            memoryCache.Remove("popular_courses");
            await _courseRepo.UpdateAsync(course);
        }

        public async Task DeleteAsync(string id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            // Use SoftDeleteAsync instead of DeleteAsync for soft deletion
            await _courseRepo.SoftDeleteAsync(course);
            // Invalidate the cache
            memoryCache.Remove("popular_courses");
            await _courseRepo.DeleteAsync(course);
        }

        public async Task<VideoDetailsDTO> GetVideoAsync(string coursePath, int unitOrderIndex, int videoOrderIndex)
        {
            var video = await videoRepo.GetVideoByCoursePathAndIndicesAsync(coursePath, unitOrderIndex, videoOrderIndex);

            if (video == null)
            {
                return null; // Or throw an exception, depending on your error handling strategy
            }
            var videoDto = mapper.Map<VideoDetailsDTO>(video);
            return videoDto;

        }

        public async Task<CourseContentDTO> GetCourseContentAsync(string coursePath, string userId = null)
        {
            var course = await _courseRepo.GetCourseByPathAsync(coursePath,userId);

            if (course == null)
                return null;

            var dto = mapper.Map<CourseContentDTO>(course);

            //// Add user-specific progress if userId is provided
            //if (!string.IsNullOrEmpty(userId))
            //{
            //    var userCourse = course.CourseAccounts.FirstOrDefault(ca => ca.UserId == userId);
            //    if (userCourse != null)
            //    {
            //        // Here you would map video completion status
            //        // Requires additional data model for tracking video progress
            //        dto.progress = userCourse.Progress;
            //    }
            //}

            return dto;
        }
        public async Task<CourseDetailsDTO> GetCourseDetailsForEditAsync(string id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

            // Map to CourseDetailsDTO
            var courseDto = mapper.Map<CourseDetailsDTO>(course);

            // Get the category name
            var category = await categoryRepo.GetByIdAsync(course.CategoryID);
            if (category != null)
            {
                courseDto.CategoryName = category.Name;
            }

            return courseDto;
        }

        public async Task<IEnumerable<MyCourseDTO>>  GetCoursesByUsernameAsync(string username)
        {
            var courses = await _courseRepo.GetCoursesByUsernameAsync(username);
            var MyCourses = mapper.Map<IEnumerable<MyCourseDTO>>(courses);
            return MyCourses;
       
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
