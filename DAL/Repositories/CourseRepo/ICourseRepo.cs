using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace DAL.Repositories.CourseRepo
{
    public interface ICourseRepo : IGenericRepository<Course>
    {
        Task<CourseAccount> GetCourseByPathAsync(string coursePath, string userId = null);
        Task<IEnumerable<CourseAccount>> GetCoursesByUsernameAsync(string username);
    }
}
