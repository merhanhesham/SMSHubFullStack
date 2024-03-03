using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities;
using SMSHub.Core.Specifications;

namespace SMSHub.Repository
{
    internal class SpecificationEvaluator<T>where T : BaseEntity
    {
        //func to build query
                                                        //yst2bel dbset
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery, ISpecifications<T> spec) {
            var query = InputQuery;//_dbcontext.Products
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy != null)//p=>p.name
            {
                query = query.OrderBy(spec.OrderBy); //_dbcontext.Products.OrderBy(p=>p.name)

            }

            if(spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if(spec.IsPaginationEnabled)
            {
                query=query.Skip(spec.Skip).Take(spec.Take);
            }

            query=spec.Includes.Aggregate(query,(CurrentQuery,IncludeExp) => CurrentQuery.Include(IncludeExp));
            return query;
        
        }
    }
}
