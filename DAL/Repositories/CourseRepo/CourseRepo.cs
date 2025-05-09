using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.DB_Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.CourseRepo
{
    public class CourseRepo : GenericRepository<Course>, ICourseRepo
    {
        public CourseRepo(E_LearningDB context) : base(context) { }
        public async Task<Course> GetCourseByPathAsync(string coursePath, string userId = null)
        {
            var course = await _dbContext.Courses
                .AsNoTracking()
                .Include(c => c.CourseUnits)
                    .ThenInclude(u => u.videos.ToList().OrderBy(v => v.order))
                .Include(c => c.CourseAccounts)
                .FirstOrDefaultAsync(c => c.Path == coursePath);

            if (course == null)
                return null;



            return course;
        }


    }
}
