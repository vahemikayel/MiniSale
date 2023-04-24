using MiniSale.Generic.Repository.Models;
using MiniSale.Generic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSale.Generic.Repository.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T, TIdentity> Repository<T, TIdentity>()
            where T : BaseEntity<TIdentity>
            where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>;

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        IUnitOfWork GetFactory(CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<DateTime> GetDateTimeFromSqlServer(CancellationToken cancellationToken);
    }
}
