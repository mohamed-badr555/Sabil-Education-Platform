using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Rate
    {
        public object CourseId;

        public class Rating
        {
            public int Id { get; set; }
            public int CourseId { get; set; }
            public int Value { get; set; } // تقييم من 1 إلى 5
            public Course Course { get; set; }
        }

    }
}
