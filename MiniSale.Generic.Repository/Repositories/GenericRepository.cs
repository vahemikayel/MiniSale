using Microsoft.EntityFrameworkCore;
using MiniSale.Generic.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniSale.Generic.Repository.Repositories
{
    public class GenericRepository<T, TIdentity, TDBContext> : IGenericRepository<T, TIdentity>
        where T : BaseEntity<TIdentity>
        where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
        where TDBContext : DbContext, new()
    {
        private readonly TDBContext _context;// BaseDBContext<TContext> _context;

        public GenericRepository(TDBContext context)//BaseDBContext<TContext> context
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var a = await _context.Set<T>()
                                  .AddAsync(entity, cancellationToken);
            return a.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>()
                          .AddRangeAsync(entities, cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .AnyAsync(expression, cancellationToken);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                           .Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>()
                           .AsQueryable();
        }

        public T Remove(T entity)
        {
            var res = _context.Remove(entity);
            return res.Entity;
        }

        public async Task<int> Remove(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>()
                                 .Where(predicate)
                                 .ExecuteDeleteAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Set<T>()
                    .Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
