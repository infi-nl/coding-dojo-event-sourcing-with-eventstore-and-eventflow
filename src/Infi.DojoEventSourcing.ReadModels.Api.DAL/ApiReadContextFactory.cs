﻿using Infi.DojoEventSourcing.Db;

 namespace Infi.DojoEventSourcing.ReadModels.Api.DAL
{
    public static class ApiReadContextFactory
    {
        public static DatabaseReadContext<ApiReadModelRepositoryFactory> Create(string connectionString)
        {
            return new DatabaseReadContext<ApiReadModelRepositoryFactory>(
                connectionString,
                new DatabaseReadRepositoryFactoryFactory());
        }
    }
}
