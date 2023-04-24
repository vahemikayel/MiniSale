using MiniSale.Generic.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniSale.Generic.Repository.Repositories
{
    public interface IGenericRepository<T, TIdentity>
       where T : BaseEntity<TIdentity>
       where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Update(T entity);

        T Remove(T entity);

        Task<int> Remove(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        IQueryable<T> GetAll();

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
