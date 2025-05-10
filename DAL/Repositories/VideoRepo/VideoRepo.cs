using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using DAL.DB_Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.VideoRepo
{
    // DAL/Repositories/VideoRepo/VideoRepo.cs
    public class VideoRepo : GenericRepository<Video>, IVideoRepo
    {
        public VideoRepo(E_LearningDB context) : base(context) { }

        public async Task<Video> GetVideoByCoursePathAndIndicesAsync(string coursePath, int unitOrderIndex, int videoOrderIndex)
        {
            return await _dbContext.Videos
                .Include(v => v.CourseUnit)
                .ThenInclude(u => u.Course)
                .FirstOrDefaultAsync(v =>
                    v.CourseUnit.Course.Path == coursePath &&
                    v.CourseUnit.Order == unitOrderIndex &&
                    v.order == videoOrderIndex);
        }

        // Add method to check for duplicate order, including soft-deleted records
        public async Task<bool> OrderExistsInUnitAsync(string unitId, int order, string videoIdToExclude = null)
        {
            var query = _dbContext.Videos
                .Where(v => v.CourseUnitID == unitId && v.order == order);

            // Exclude the current video if we're checking during an update
            if (!string.IsNullOrEmpty(videoIdToExclude))
            {
                query = query.Where(v => v.Id != videoIdToExclude);
            }

            return await query.AnyAsync();
        }
    }
}
