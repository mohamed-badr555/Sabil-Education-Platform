using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Managers.EnrollmentManager
{
    public class EnrollmentManager : IEnrollmentManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Course> courseRepo;
        private readonly IGenericRepository<CourseAccount> enrollmentRepo;

        public EnrollmentManager(
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Course> courseRepo,
            IGenericRepository<CourseAccount> enrollmentRepo)
        {
            _userManager = userManager;
            this.courseRepo = courseRepo;
            this.enrollmentRepo = enrollmentRepo;
        }

        public async Task<bool> EnrollAccountAsync(string userId, int courseId)
        {
            // Check if the user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            // Check if the course exists
            var course = await courseRepo.GetByIdAsync(courseId);
            if (course == null)
                return false;

            // Check if already enrolled
            var alreadyEnrolled = await enrollmentRepo.AnyAsync(e =>
                e.UserId ==userId && e.CourseID == courseId);

            if (alreadyEnrolled)
                return false;

            // Create the enrollment record
            var enrollment = new CourseAccount
            {
                UserId = userId,
                CourseID = courseId,
                StartDate = DateTime.Now
            };

            try
            {
                await enrollmentRepo.InsertAsync(enrollment);
                return true;
            }
            catch
            {
             
                return false;
            }
        }
    }

}
