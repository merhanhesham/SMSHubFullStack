using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities;

namespace SMSHub.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; }
        public List<Expression<Func<T,object>>> Includes { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; }

        //prop for order by [OrderByDesc(p=>p.name)]
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        //take(2)
        public int Take { get; set; }
        //skip(2)
        public int Skip { get; set; }

        public bool IsPaginationEnabled { get; set; }
    }
}
