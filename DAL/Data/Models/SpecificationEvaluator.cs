﻿using DAL;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Data.Repository.Models
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }


            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
