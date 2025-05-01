using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Data.Models;

namespace BLL.Specifications.Courses
{
    public class CourseSpecifications : BaseSpecification<Course>
    {
        public CourseSpecifications(CourseSpecsParams courseParams)
            : base(c =>
    (string.IsNullOrEmpty(courseParams.Search) ||
                    c.Title.ToLower().Contains(courseParams.Search) ||
                    c.Description.ToLower().Contains(courseParams.Search) ||
                    c.Details.ToLower().Contains(courseParams.Search)) &&
                (!courseParams.CategoryId.HasValue ||
                    c.CategoryID == courseParams.CategoryId.Value) &&
                (string.IsNullOrEmpty(courseParams.Level) ||
                    c.Level == courseParams.Level)
                 )
        {
            AddInclude(c => c.Category);
            AddOrderBy(c => c.Title);

            // PageIndex = 2
            // PageSize = 5

            ApplyPagination(courseParams.PageSize * (courseParams.PageIndex - 1), courseParams.PageSize);

            if (!string.IsNullOrEmpty(courseParams.Sort))
            {
                switch (courseParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(c => c.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(c => c.Price);
                        break;
                    default:
                        AddOrderBy(c => c.Title);
                        break;
                }
            }
        }

        public CourseSpecifications(int id) : base(P => P.Id == id)
        {
            AddInclude(c => c.Category);
            AddOrderBy(c => c.Title);
        }
    }
}

