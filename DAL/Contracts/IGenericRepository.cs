using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int? id);
        Task Post(T obj);
        Task Update(T obj);
        Task Delete(int? id);
        Task Save();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);
    }
}
