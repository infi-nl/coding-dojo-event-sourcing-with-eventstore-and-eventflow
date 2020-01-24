using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Infi.DojoEventSourcing.Db
{
    public abstract class AbstractDatabaseContext<TRepositoryFactory> : IDatabaseContext<TRepositoryFactory>
        where TRepositoryFactory : IRepositoryFactory
    {
        public abstract Task RunAsync(Func<TRepositoryFactory, Task> action);

        public abstract Task<TResult> RunAsync<TResult>(
            Func<TRepositoryFactory, Task<TResult>> databaseContextFunction);

        public virtual async Task RunTransactionAsync(
            Func<TRepositoryFactory, TransactionScope, Task> transactionalAction,
            IsolationLevel isolationLevel)
        {
            await RunTransactionAsync(
                (repositoryFactory, transaction) =>
                    Task.Run(() => transactionalAction(repositoryFactory, transaction)),
                isolationLevel);
        }

        public abstract Task<TResult> RunTransactionAsync<TResult>(
            Func<TRepositoryFactory, TransactionScope, Task<TResult>> transactionalFunction,
            IsolationLevel isolationLevel);
    }
}