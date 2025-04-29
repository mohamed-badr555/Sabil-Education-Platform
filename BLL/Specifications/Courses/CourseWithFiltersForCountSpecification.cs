using System;
using DAL;
using DAL.Data.Models;

namespace BLL.Specifications.Courses
{
    public class CourseWithFiltersForCountSpecification : BaseSpecification<Course>
    {
        public CourseWithFiltersForCountSpecification(CourseSpecsParams courseParams)
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
            // No includes, sorting, or pagination here — just filtering.
        }
    }
}
