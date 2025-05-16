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
                    (c.Title != null && c.Title.ToLower().Contains(courseParams.Search)) ||
                    (c.Description != null && c.Description.ToLower().Contains(courseParams.Search)) ||
                    (c.Details != null && c.Details.ToLower().Contains(courseParams.Search))) &&
                (string.IsNullOrEmpty(courseParams.CategoryId) ||
                    c.CategoryID == courseParams.CategoryId) &&
                (string.IsNullOrEmpty(courseParams.Level) ||
                    c.Level == courseParams.Level) &&
                (!courseParams.IsFree.HasValue || c.IsFree == courseParams.IsFree.Value)
            )
        {
            // No includes, sorting, or pagination here — just filtering
        }
    }
}
