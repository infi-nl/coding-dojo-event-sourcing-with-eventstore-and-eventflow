# DojoEventSourcing
Largely inspired by: https://github.com/luontola/cqrs-hotel

## Getting started
1. Create the `appsetting.json` files by copying the `appsetting.Example.json` files in 
    * `Infi.DojoEventSourcing.Api`
    * `Infi.DojoEventSourcing.ReadModelDbMigrator`
2. We'll be working with [EventStore](https://eventstore.com/). You'll need an instance to write our events to. 
    * The easiest way to do this is running `docker-compose up` from the root directory in this repository. 
    * You can also manually install EventStore by following the instuctions [here](https://eventstore.com/docs/getting-started/?tabs=tabid-3%2Ctabid-dotnet-client%2Ctabid-dotnet-client-connect%2Ctabid-4).
    * We also provide a shared instance. Ask us for the credentials. This is less ideal, because you'll see the events of other participants mixed up with yours.
    * EventStores comes with a UI, which can be found at http://localhost:2113. You can login with the default username _admin_  and password _changeit_
3. Once you have an EventStore instance running, you must provide the connection credentials in the `Infi.DojoEventSourcing.Api/appsettings.json`. If you've used the default settings, you're ok already.
4. Besides EventStore we'll also need a database for our read models. In this exercise we'll use SQLite.
    * Create a `readmodel.db` file somewhere
    * Add the db file path to the `Infi.DojoEventSourcing.Api/appsettings.json`
    * Add the db file path to the `Infi.DojoEventSourcing.ReadModelDbMigrator/appsettings.json`
5. Build and run the `Infi.DojoEventSourcing.ReadModelDbMigrator` program to generate the required schema for your readmodels.
6. It would be nice if you could inspect the database somehow. Rider has build in support for SQLite databases. Another client can be found [here](https://sqlitebrowser.org/)
7. Build and run the `Infi.DojoEventSourcing.Api` program, you sould be ready for the exercise now.

