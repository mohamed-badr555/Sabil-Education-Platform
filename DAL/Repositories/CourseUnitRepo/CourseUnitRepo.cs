using DAL.Data.Models;
using DAL.DB_Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.CourseUnitRepo
{
    // DAL/Repositories/CourseUnitRepo/CourseUnitRepo.cs
    public class CourseUnitRepo : GenericRepository<CourseUnit>, ICourseUnitRepo
    {
        public CourseUnitRepo(E_LearningDB context) : base(context) { }

        public async Task<IEnumerable<CourseUnit>> GetUnitsByCourseIdAsync(string courseId)
        {
            return await _dbContext.CourseUnits
                .AsNoTracking()
                .Where(u => u.CourseID == courseId && !u.IsDeleted)
                .Include(u => u.videos.Where(v => !v.IsDeleted))
                .OrderBy(u => u.Order)
                .ToListAsync();
        }

        // Add method to check for duplicate order, including soft-deleted records
        public async Task<bool> OrderExistsInCourseAsync(string courseId, int order, string unitIdToExclude = null)
        {
            var query = _dbContext.CourseUnits
                .Where(u => u.CourseID == courseId && u.Order == order);

            // Exclude the current unit if we're checking during an update
            if (!string.IsNullOrEmpty(unitIdToExclude))
            {
                query = query.Where(u => u.Id != unitIdToExclude);
            }

            return await query.AnyAsync();
        }
    }
}
