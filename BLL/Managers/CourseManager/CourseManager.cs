using AutoMapper;
using BLL.DTOs;
using BLL.Specifications.Courses;
using DAL.Data.Models;
using DAL.Repositories;
using DAL.Repositories.CourseRepo;
using DAL.Repositories.VideoRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

        public CourseManager(ICourseRepo courseRepo, IGenericRepository<Category> categoryRepo, IVideoRepo _videoRepo, IMapper mapper , IMemoryCache memoryCache)
        {
            _courseRepo = courseRepo;
            this.categoryRepo = categoryRepo;
            videoRepo = _videoRepo;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
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

        public async Task AddAsync(CourseAddDTO course_dto)
        {
            
            var course = mapper.Map<Course>(course_dto);
            await _courseRepo.InsertAsync(course);
        }


        public async Task UpdateAsync(CourseAddDTO CourseDTO)
        {

   
            var course = await _courseRepo.GetByIdAsync(CourseDTO.Id);
      
            if (course == null)
                throw new Exception("Course not found");

            mapper.Map(CourseDTO, course); // maps *into* the existing object

            // Invalidate the cache
            memoryCache.Remove("popular_courses");
            await _courseRepo.UpdateAsync(course);
        }


        public async Task DeleteAsync(string id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {id} not found.");

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
