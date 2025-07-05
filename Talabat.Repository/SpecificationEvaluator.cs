using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> Inputquery,ISpecifications<TEntity> spec)
        {
            var query = Inputquery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
