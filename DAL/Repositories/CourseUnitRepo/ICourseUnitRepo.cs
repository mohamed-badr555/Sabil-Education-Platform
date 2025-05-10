using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.CourseUnitRepo
{
    public interface ICourseUnitRepo : IGenericRepository<CourseUnit>
    {
        Task<IEnumerable<CourseUnit>> GetUnitsByCourseIdAsync(string courseId);
        Task<bool> OrderExistsInCourseAsync(string courseId, int order, string unitIdToExclude = null);
    }
}
