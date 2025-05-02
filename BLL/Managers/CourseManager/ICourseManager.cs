using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTOs;
using BLL.Specifications.Courses;

namespace BLL.Managers.CourseManager
{
    public interface ICourseManager
    {
        Task<Pagination<CourseListDTO>> GetAllAsync(CourseSpecsParams courseparams);
        Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync();
        Task<Pagination<CourseListDTO>> GetPopularAsync();
        Task<CourseDetailsDTO> GetByIdAsync(string id);
        Task<List<CourseListDTO>> SearchCoursesAsync(string SearchItem);
        Task<List<CourseListDTO>> FilterCoursesAsync(string level, string category);
        Task AddAsync(CourseAddDTO Course);
        Task UpdateAsync(CourseAddDTO Course);
        Task DeleteAsync(string id);
    }
}
