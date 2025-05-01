using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Specifications.Courses
{
    public class CourseSpecsParams
    {

        public const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string? Sort { get; set; }
        public int? CategoryId { get; set; }
        public string? Level { get; set; }


        public bool? IsFree { get; set; }


        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }


    }
}
