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

        private int pageIndex = 1;
        public int PageIndex
        {
            get => pageIndex;
            set => pageIndex = (value <= 0) ? 1 : value;
        }

        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : (value <= 0) ? 5 : value;
        }

        public string? Sort { get; set; }
        public string? CategoryId { get; set; }
        public string? Level { get; set; }
        public bool? IsFree { get; set; }

        private string? search;
        public string? Search
        {
            get => search;
            set => search = value?.ToLower(); // Added null-conditional operator to prevent NullReferenceException
        }
    }
}
