using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities;
using SMSHub.Core.Specifications;

namespace SMSHub.Core.Repositories
{
    public interface IGenericRepository<T>where T:BaseEntity //kda fhemto en T lazm ykon class w ykon leh table f db
    {
        //use IReadOnlyList instead of Ienumerable if i don't need to iterate bec it's better for performance
        #region without spec
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region with spec
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T> GetByIdWithSpecAsync(ISpecifications<T> spec);

        Task<T> GetCountWithSpecAsync(ISpecifications<T> spec);
        #endregion

        public Task AddAsync(T item);
        public void Update(T item);
        public void Delete(T item);
    }
}
