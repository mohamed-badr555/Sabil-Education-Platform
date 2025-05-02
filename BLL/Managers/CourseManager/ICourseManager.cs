using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Specifications.Courses;
using DAL.Data.Models;

namespace BLL.Managers.CourseManager
{
    public interface ICourseManager
    {
        object Courses { get; set; }
        object Rates { get; }

        Task<Pagination<CourseListDTO>> GetAllAsync(CourseSpecsParams courseparams);
        Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync();
        Task<Pagination<CourseListDTO>> GetPopularAsync();
        Task<CourseDetailsDTO> GetByIdAsync(int id);
        Task<List<CourseListDTO>> SearchCoursesAsync(string SearchItem);
        Task<List<CourseListDTO>> FilterCoursesAsync(string level, string category);
        Task AddAsync(CourseAddDTO Course);
        Task UpdateAsync(CourseAddDTO Course);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
        Task<Course> AddRateAsync(string coursePath, Rate rate);
    }
}
