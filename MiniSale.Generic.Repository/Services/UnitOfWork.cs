using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniSale.Generic.Repository.Models;
using MiniSale.Generic.Repository.Repositories;
using System.Collections;
using System.Data.Common;

namespace MiniSale.Generic.Repository.Services
{
    public class UnitOfWork<TDBContext> : IUnitOfWork, IDisposable, IAsyncDisposable
       where TDBContext : DbContext, new()
    {
        private readonly Hashtable _repositories;
        private IDbContextTransaction _transaction;
        private TDBContext _baseDBContext { get; set; }

        public UnitOfWork(TDBContext baseDBContext)
        {
            _repositories = new Hashtable();
            _baseDBContext = baseDBContext ?? throw new ArgumentNullException(nameof(baseDBContext));
        }

        public DbConnection GetConnection()
        {
            return _baseDBContext.Database.GetDbConnection();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _baseDBContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _baseDBContext.Database.CommitTransactionAsync(cancellationToken);
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Rollback();
            _baseDBContext.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            //if (_transaction != null && _transaction.)
            //    await _transaction?.RollbackAsync();

            await _baseDBContext.DisposeAsync();
        }

        public IUnitOfWork GetFactory(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _baseDBContext.Database.RollbackTransactionAsync(cancellationToken);
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _baseDBContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<DateTime> GetDateTimeFromSqlServer(CancellationToken cancellationToken)
        {
            var connection = GetConnection();
            using (var cmd = connection.CreateCommand())
            {
                await connection.OpenAsync(cancellationToken);
                cmd.CommandText = "SELECT GETDATE()";
                var res = await cmd.ExecuteScalarAsync(cancellationToken);
                await connection.CloseAsync();
                return (DateTime)res;
            }
        }

        public IGenericRepository<T, TIdentity> Repository<T, TIdentity>()
            where T : BaseEntity<TIdentity>
            where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>
        {
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T, TIdentity, TDBContext>);
                var repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { _baseDBContext });

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T, TIdentity>)_repositories[type];
        }

    }
}
