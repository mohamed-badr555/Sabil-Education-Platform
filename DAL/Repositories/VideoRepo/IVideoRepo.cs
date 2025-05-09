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
        public Task<Video> GetVideoByCoursePathAndIndicesAsync(string coursePath, int unitOrderIndex, int videoOrderIndex);
    }
}
