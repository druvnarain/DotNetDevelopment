using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, 
        ISpecification<TEntity> spec)
        {
            //inputQuery is from _context.Set<T>().AsQueryable(), basically the queried table <T>
            var query = inputQuery;

            //Start searching the table based on criteria in spec
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);  // p => p.ProductTypeId == id
            }

            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);  
            }

            if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);  
            }

            //Skip and Take are build in methods for Iqueryable Linq. Skip x elements, return y elements after
            if(spec.IsPagingEnabled) 
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            //Includes is a list of expressions. Aggregrate is used on a sequences (list in this case)
            //Aggregate(startingValue(s), function to be applied to startingValue, (optional) function that is applied to final result )
            //current represents the query expression, include represents arrow function in Includes list
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}