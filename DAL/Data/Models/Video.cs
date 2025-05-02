using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Video :BaseEntity
    {
       
        [MaxLength(75)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string? Description { get; set; }
        [MaxLength(1500)]
        public string URL { get; set; }
        public int order { get; set; }

        #region Video - CourseUnitID (M-1)
        public string CourseUnitID { get; set; } //Foreign key
        public CourseUnit CourseUnit { get; set; }
        #endregion

        #region Video  - VideoComments (M-M)
        public ICollection<VideoComment> VideoComments { get; set; }
        #endregion

        #region Video - Exam (1-1)
        public string? ExamID { get; set; }
        public Exam Exam { get; set; }
        #endregion
    }
}
