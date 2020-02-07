# DojoEventSourcing
This dojo is targeted at a people that are somewhat experienced in C#, and have some knowledge about what Event Sourcing is and how it works, but never got around to applying it in practice.

We've made a basic assignment for you that implements a simple Hotel booking system. You'll learn to think in terms of events instead of state. This assignment uses [EventStore](https://eventstore.com/) as a datastore for events. We also use the [EventFlow](https://github.com/eventflow/EventFlow) framework.

**Some useful EventSourcing articles**
* https://dev.to/barryosull/event-sourcing-what-it-is-and-why-its-awesome
* https://arkwright.github.io/event-sourcing.html

This application is largely inspired by: https://github.com/luontola/cqrs-hotel

## Getting started
1. Create the `appsetting.json` files by copying the `appsetting.Example.json` files in 
    * `Infi.DojoEventSourcing.Api`
    * `Infi.DojoEventSourcing.ReadModelDbMigrator`
2. We'll be working with [EventStore](https://eventstore.com/). You'll need an instance to write our events to. 
    * The easiest way to do this is running `docker-compose up` from the root directory in this repository. 
    * You can also manually install EventStore by following the instuctions [here](https://eventstore.com/docs/getting-started/?tabs=tabid-3%2Ctabid-dotnet-client%2Ctabid-dotnet-client-connect%2Ctabid-4).
    * We also provide a shared instance. Ask us for the credentials. This is less ideal, because you'll see the events of other participants mixed up with yours.
    * EventStores comes with a GUI, which can be found at http://localhost:2113. You can login with the default username _admin_  and password _changeit_
3. Once you have an EventStore instance running, you must provide the connection credentials in the `Infi.DojoEventSourcing.Api/appsettings.json`. If you've used the default settings, you're ok already.
4. Besides EventStore we'll also need a database for our read models. In this exercise we'll use SQLite.
    * Create a `readmodel.db` file somewhere
    * Add the db file path to the `Infi.DojoEventSourcing.Api/appsettings.json`
    * Add the db file path to the `Infi.DojoEventSourcing.ReadModelDbMigrator/appsettings.json`
5. Build and run the `Infi.DojoEventSourcing.ReadModelDbMigrator` program to generate the required schema for your readmodels.
6. It would be nice if you could inspect the database somehow. Rider has build in support for SQLite databases. Another client can be found [here](https://sqlitebrowser.org/)
7. Build and run the `Infi.DojoEventSourcing.Api` program, you sould be ready for the exercise now.

n.b. To use the api something like [Postman](https://www.postman.com/) could come in handy.

## Getting familiar with EventStore
=> Go to the EventStore GUI http://localhost:2113
You'll see the dashboard, which shows some technical information and the current open connections.

=> Go to the Stream Browser page. You wont see much here yet, but this will be you're main entry point to peek inside the EventStore.

=> Go to the Projections page. In order to browse streams, we first need to enable the `$streams` projection, by clicking on `$streams` and then on `start` in the right corner.

Now it's time to create our first stream. Make sure you started the `Infi.DojoEventSourcing.Api` and make the following call 
```
[POST] http://localhost:5000/Room/CreateRoom
{
   "Number": "1"
}
```

Refresh the Stream Browser in the ES GUI, and you'll see a newly created room stream. Click on it to see all the events that belong to that stream. You'll see one event: `RoomCreated`. If you expand it, you'll see the data for that event in json format. Each room will have it's own event stream and all events for that specific room will be collected in its event stream. So when we make a reservation that occupies this room, a `RoomOccupied` event will be stored in this stream.

## Getting familiar with EventFlow
EventFlow is a CQRS + EventSourcing framework that can use a variety of event stores (i.e. EventStore) and read stores (i.e. Sqlite). It makes it easy to manage aggregates, apply events on them and maintain different event versions. The best way to understand the basics, is to walk through the code that created our first room.
1. Open the `RoomController` and go to the `CreateRoom` method.
   We use the EF `CommandBus` to publish a `CreateRoom` _command_ with a newly generated room command.
   If this command succeeds, we return the id, otherwise something went wrong and we return a BadRequest
2. Let's find out where this command is processed. Go to the `CreateRoomHandler`.
   Everytime the `CommandBus` retrieves a `CreateRoom` command it will:
      * Instantiate a new `Room` object with that room id
      * Retrieve and apply all existing events for that room id from ES, none in this case
      * Instantiate a `CreateRoomHandler` and call `ExecuteAsync` with the hydrated `Room` object and the published command.
3. Follow the `room.Create` call into the Room aggregate, you'll see there's a `RoomCreated` event emitted here. Typically this is the place where you'd first do some validation. Emitting the event won't be committed to the event store yet. This will only happen once the calling command handler end with a succesful result.
4. Open the `RoomCreated` event. This class corresponds with the data that we found in the EventStore GUI.
