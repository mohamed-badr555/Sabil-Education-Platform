using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace DAL.Repositories.VideoRepo
{
    public interface IVideoRepo : IGenericRepository<Video>
    {
        Task<Video> GetVideoByCoursePathAndIndicesAsync(string coursePath, int unitOrderIndex, int videoOrderIndex);
        Task<bool> OrderExistsInUnitAsync(string unitId, int order, string videoIdToExclude = null);
    }
}
