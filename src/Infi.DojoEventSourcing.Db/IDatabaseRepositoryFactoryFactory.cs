using System.Data.Common;

namespace Infi.DojoEventSourcing.Db
{
    public interface IDatabaseRepositoryFactoryFactory<out TRepositoryFactory>
        where TRepositoryFactory : IRepositoryFactory
    {
        TRepositoryFactory Create(DbConnection connection);
    }
}
