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
        Task<CourseContentDTO> GetByIdAsync(string id);

        Task AddAsync(CourseAddDTO Course);
        Task UpdateAsync(CourseAddDTO Course);
        Task DeleteAsync(string id);
        Task<VideoDetailsDTO> GetVideoAsync(string coursePath,int unitOrderIndex, int videoOrderIndex);
        Task<CourseContentDTO> GetCourseContentAsync(string coursePath, string userId = null);
    }
}
