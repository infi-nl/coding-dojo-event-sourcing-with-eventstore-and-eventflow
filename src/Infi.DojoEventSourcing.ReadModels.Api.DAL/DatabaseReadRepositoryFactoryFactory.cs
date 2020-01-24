using System.Data.Common;
using Infi.DojoEventSourcing.Db;

namespace Infi.DojoEventSourcing.ReadModels.Api.DAL
{
    public class DatabaseReadRepositoryFactoryFactory : IDatabaseRepositoryFactoryFactory<ApiReadModelRepositoryFactory>
    {
        public ApiReadModelRepositoryFactory Create(DbConnection connection)
        {
            return new ApiReadModelRepositoryFactory(connection);
        }
    }
}