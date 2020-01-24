using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Infi.DojoEventSourcing.Db
{
    public interface IDatabaseContext<out TRepositoryFactory>
        where TRepositoryFactory : IRepositoryFactory
    {
        Task RunAsync(Func<TRepositoryFactory, Task> action);
        Task<TResult> RunAsync<TResult>(Func<TRepositoryFactory, Task<TResult>> databaseContextFunction);

        Task RunTransactionAsync(
            Func<TRepositoryFactory, TransactionScope, Task> transactionalAction,
            IsolationLevel isolationLevel);

        Task<TResult> RunTransactionAsync<TResult>(
            Func<TRepositoryFactory, TransactionScope, Task<TResult>> transactionalFunction,
            IsolationLevel isolationLevel);
    }
}