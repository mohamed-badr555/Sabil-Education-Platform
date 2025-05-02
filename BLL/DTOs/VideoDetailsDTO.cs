using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;

namespace BLL.DTOs
{
    public class VideoDetailsDTO
    {

        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string URL { get; set; }
        public int order { get; set; }
        public string? ExamID { get; set; }
        public int unitorder { get; set; }

        public string CourseUnitID { get; set; } 

        public ICollection<VideoComment> VideoComments { get; set; }

    }
}
