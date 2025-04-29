using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Data.Models;

namespace BLL.Specifications.Courses
{
    public class PopularCourseSpecification : BaseSpecification<Course>
    {
        public PopularCourseSpecification()
            : base() 
        {
            AddInclude(c => c.Category); // optional 
            AddOrderByDescending(c => c.Rating);
            ApplyPagination(0, 5); // get top 5
        }
    }

}
