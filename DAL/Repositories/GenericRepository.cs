using DAL.Contracts;
using DAL.EF;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T: BaseEntity
    {
        private readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int? id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Post(T obj)
        {
            context.Set<T>().Add(obj);
            await Save();
        }

        public async Task Update(T obj)
        {
            var local = context.Set<T>().Local.FirstOrDefault(f => f.Id == obj.Id);
            if(local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            else
            {
                throw new ArgumentException("Not found");
            }

            context.Entry(obj).State = EntityState.Modified;
            await Save();
        }

        public async Task Delete(int? id)
        {
            var deleted = await GetById(id);
            if (deleted != null)
                context.Set<T>().Remove(deleted);
            else
                throw new ArgumentException("Not found");
            await Save();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
