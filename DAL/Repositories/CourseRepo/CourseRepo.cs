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
        public async Task<CourseAccount> GetCourseByPathAsync(string coursePath, string userId = null)
        {
            var course = await _dbContext.CourseAccounts
                .AsNoTracking()
                .Include(ca=>ca.course)
                .ThenInclude(c => c.CourseUnits)
                .ThenInclude(u => u.videos.ToList().OrderBy(v => v.order))
                .FirstOrDefaultAsync(ca => ca.course.Path == coursePath);

            if (course == null)
                return null;



            return course;
        }

        public async Task<IEnumerable<CourseAccount>> GetCoursesByUsernameAsync(string username)
        {
            return await _dbContext.CourseAccounts
                .Include(ca=>ca.course)
                .ThenInclude(c=>c.Category)
                .Where(ca => ca.User.UserName == username)
                .ToListAsync();
        }


    }
}
