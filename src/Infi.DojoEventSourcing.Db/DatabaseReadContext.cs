using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.Sqlite;
using Serilog;

namespace Infi.DojoEventSourcing.Db
{
    public class DatabaseReadContext<TRepositoryFactory> : AbstractDatabaseContext<TRepositoryFactory>
        where TRepositoryFactory : IRepositoryFactory
    {
        private readonly string _connectionString;
        private readonly IDatabaseRepositoryFactoryFactory<TRepositoryFactory> _repositoryFactoryFactory;

        public DatabaseReadContext(
            string connectionString,
            IDatabaseRepositoryFactoryFactory<TRepositoryFactory> repositoryFactoryFactory)
        {
            _connectionString = connectionString;
            _repositoryFactoryFactory = repositoryFactoryFactory;
        }

        public override async Task RunAsync(Func<TRepositoryFactory, Task> databaseContextFunction)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await databaseContextFunction(_repositoryFactoryFactory.Create(connection));
            }
        }

        public override async Task<TResult> RunAsync<TResult>(
            Func<TRepositoryFactory, Task<TResult>> databaseContextFunction)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await databaseContextFunction(_repositoryFactoryFactory.Create(connection));
            }
        }

        public override async Task<TResult> RunTransactionAsync<TResult>(
            Func<TRepositoryFactory, TransactionScope, Task<TResult>> transactionalFunction,
            IsolationLevel isolationLevel)
        {
            using (var transaction = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    try
                    {
                        return await transactionalFunction(_repositoryFactoryFactory.Create(connection), transaction);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "An exception occurred while running a transaction");
                        throw;
                    }
                }
            }
        }
    }
}