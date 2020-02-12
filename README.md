# DojoEventSourcing
This dojo is targeted at people that are somewhat experienced in C#, and have some knowledge about what Event Sourcing is and how it works, but never got around to applying it in practice.

We've made a basic assignment for you that implements a simple Hotel booking system. You'll learn to think in terms of events instead of state. This assignment uses [EventStore](https://eventstore.com/) as a datastore for events. We also use the [EventFlow](https://github.com/eventflow/EventFlow) framework.

## Acknowledgements
This application is largely inspired by: https://github.com/luontola/cqrs-hotel

## Background and useful articles on EventSourcing
* https://dev.to/barryosull/event-sourcing-what-it-is-and-why-its-awesome
* https://arkwright.github.io/event-sourcing.html

## Requirements
- dotnet core 3.0
- docker
- optional: docker-compose

## Getting started
1. Create the `appsettings.json` files by copying the `appsettings.Example.json` files in 
    * `Infi.DojoEventSourcing.Api`
    * `Infi.DojoEventSourcing.ReadModelDbMigrator`
2. We'll be working with [EventStore](https://eventstore.com/). You'll need an instance to write our events to. Choose one of the following options:
    * OR: Run `docker-compose up` from the root directory in this repository.
    * OR: Install EventStore manually by following the instuctions [here](https://eventstore.com/docs/getting-started/).
3. Make sure you start EventStore with projections enabled -- **for all non-Docker-installations**, this requires adding one or two command line parameters when starting the instance. This is described [here](https://eventstore.com/docs/getting-started/projections/), under _Setting up projections_.
EventStore comes with a GUI, which can be found at http://localhost:2113. You can login with the default username _admin_  and password _changeit_
4. Once you have an EventStore instance running, you must provide the connection credentials in the `Infi.DojoEventSourcing.Api/appsettings.json`. If you've used the default settings, you're ok already.
5. Besides EventStore we'll also need a database for our read models. In this exercise we'll use SQLite.
    * Create a `readmodel.db` file somewhere
    * Add the db file path to the `Infi.DojoEventSourcing.Api/appsettings.json`
    * Add the db file path to the `Infi.DojoEventSourcing.ReadModelDbMigrator/appsettings.json`
6. Build and run the `Infi.DojoEventSourcing.ReadModelDbMigrator` program to generate the required schema for your readmodels.
7. It would be nice if you could inspect the database somehow. Rider has build in support for SQLite databases. Another client can be found [here](https://sqlitebrowser.org/).
8. Build and run the `Infi.DojoEventSourcing.Api` program, you should be ready for the exercises now.

n.b. To use the api something like [Postman](https://www.postman.com/) could come in handy.
You can find Postman collection and environment files in the `postman` folder in the root of this repository.

## Getting familiar with EventStore
1. Go to the EventStore GUI http://localhost:2113, where you can login with username _admin_ and password _changeit_. You'll see the dashboard, which shows some technical information and the current open connections.
2. Go to the Stream Browser page. You won't see much here yet, but this will be your main entry point to peek inside the EventStore.
    * If the _Stream Browser_ menu link is grayed out, your EventStore instance needs to be restarted with projections. See step 3 of Getting started :point_up:.
3. Go to the Projections page. In order to browse streams, we first need to enable the `$streams` projection, by clicking on `$streams` and then on `start` in the right corner.
    * If the _Projections_ menu link is grayed out, your EventStore instance needs to be restarted with projections. See step 3 of Getting started :point_up:.
4. Now it's time to create our first stream. Make sure you started the `Infi.DojoEventSourcing.Api` and make the following call 
```
[POST] http://localhost:5000/Room
{
   "Number": "1"
}
```

Refresh the Stream Browser in the ES GUI, and you'll see a newly created Room stream. Click on it to see all the events that belong to that stream. You'll see one event: `RoomCreated`. If you expand it, you'll see the data for that event in json format. Each room will have it's own event stream and all events for that specific room will be collected in its event stream. So when we make a reservation that occupies this room, a `RoomOccupied` event will be stored in this stream.

## Getting familiar with EventFlow
EventFlow is a CQRS + EventSourcing framework that can use a variety of event stores (e.g. EventStore) and read stores (e.g. Sqlite). It makes it easy to manage aggregates, apply events to them and maintain different event versions. The best way to understand the basics, is to walk through the code that created our first room.
1. Open the `RoomController` and go to the `CreateRoom` method.
   We use the EF `CommandBus` to publish a `CreateRoom` _command_ with a newly generated room command.
   If this command succeeds, we return the id, otherwise something went wrong and we return a BadRequest
2. Let's find out how this command is processed. Everytime the `CommandBus` retrieves a `CreateRoom` command it will:
      * Instantiate a new `Room` object with that room id
      * Retrieve and apply all existing events for that room id from ES, none in this case. This process is called hydrating.
      * Instantiate a `CreateRoomHandler` and call `ExecuteAsync` with the hydrated `Room` object and the published command.
3. Go to the `CreateRoomHandler` and follow the `room.Create` call into the Room aggregate, you'll see there's a `RoomCreated` event emitted here. Typically this is the place where you'd first do some validation. Emitting the event won't be committed to the event store yet. This will only happen once the calling command handler ends with a succesful result.
4. Open the `RoomCreated` event. This class corresponds with the data that we found in the EventStore GUI.
5. As you've seen you can create rooms. These are of course required in order to make any reservations.
6. You start a new reservation by calling `[GET]  http://localhost:5000/Reservation/New`. This won't do anything except returning a newly generated `reservationId` that you can use in subsequent calls.
7. Call `[GET] http://localhost:5000/Reservation/Offers?reservationId=<guid>&arrival=YYYY-MM-dd&departure=YYYY-MM-dd` to get a price offer for the requested period. This will generate a price offer event for each day in that period. You can locate it in the ES GUI. The offer will be valid for 30 minutes.
8. If you created offers for every day of your intended stay, you can make the reservation final by calling:
```
[POST] http://localhost:5000/Reservation
{
   "ReservationId": "<guid>",
   "Arrival": "YYYY-MM-dd",
   "Departure": "YYYY-MM-dd",
   "Name": "<string>",
   "Email": "<string>"
}
```
This will generate more events that you can explore.

## Assignments
The following assignments will show you some key event sourcing and EventFlow aspects. We advise you not to put too much effort in the details. Focus on getting familiar with the different concepts.

### 1. Add a dinner option to our reservation
We'd like to offer our customers a dinner at our hotel restaurant. The customers can choose to opt-in on the dinner deal, after their reservation is confirmed. So we need a new event on the reservation aggregate which states an opt-in for the dinner deal.

**Acceptance criteria**
* There's an api endpoint to opt in for the dinner deal
* Customers can only opt-in if the reservation is confirmed (i.e. a room is assigned to the reservation)
* Customers shouldn't be able to opt-in more than once
* The opt-in should be stored in EventStore
* The reservation readmodel should be updated accordingly

### 2. Subscribe to events
If a customer wants to dine at our restaurant, we'd better give the chefs a heads-up so they can buy enough supplies.

EventFlow offers async and sync subscribers, which you can use to _do_ something once an event has happend. Synchronous subscribers are blocking and commandbus execution will wait until all sync subscribers are done. Async subscribers will not wait.

You can find more information about subscribers [here](https://eventflow.readthedocs.io/Subscribers.html).

n.b. You need to register the subscriber in the api startup.

**Acceptance criteria**
* Use a subscriber that will log a message for the chefs when someone opts-in for dinner

### 3. Rebuilding the readmodel database
When you store everything as an event, you can still produce the current state of properties you weren't interested in at first. So if you shape your events well, you can answer any question regarding the data. Even for past comitted data. This is different from convential databses, because you can only retrieve the current state. You can read more about the business value of an event log [here](https://eventstore.com/docs/event-sourcing-basics/business-value-of-the-event-log/index.html).

For this assignment we'll be making a simple adjustment to our existing `ReservationReadModel`. We're intrested in the total costs of each stay. We can calculate this by taking the sum of `OfferPrice` in all the `LineItemCreated` events. We've already setup a `ReadModelRebuilder` application, which you can use to rebuild the redamodel for al previous made reservations.

**Acceptance criteria**
* Extend the `ReservationReadModel` with a `TotalPrice` field
* Make sure the `TotalPrice` is calculated from now on
* Rebuild the `ReservationReadModel`, so the `TotalPrice` gets calculated for existing reservations

n.b. Make sure you made a few reservations before you make any adjustments to the `ReservationReadModel` in order to see the effects of rebuilding properly.

Can you think of more use cases that require readmodel rebuilds?
- Maybe we can find out if there's a correlation between the duration of a stay and opting in for the dinner deal?
- How does the amount of generated offers relate to the total costs of a reservation?

## Bonus assignments
If you have some time left you can choose to do one or more of the following assignments. The order doesn't matter.

### Send a confirmation e-mail using sagas
We'd like to send a confirmation e-mail to the customer when the reservation is successfully placed.

We could use a subscriber here again. But another way is using a _process manager_ for _saga_. A process manager coordinates messages between different aggregates. Take a look at the `ReservationSaga`, it starts when a `ReservationCreated` event has happened and then starts a procedure to occupy a room. We could add sending the confirmation e-mail here as well.

**Acceptance criteria**
* An e-mail is send as soon as a reservation is completed, a.k.a. when a room is assigned to the reservation. Logging the recipient (name + email) with some dummy text will be sufficient for this exercise.
* Use the `ReservationSaga`

### Upgrade existing events
We got some complaints from the PR-department, we send e-mails to customers with their full name. Apparently it's policy to address them only by their first name. So we need to split the name field into two separate first name and last name fields.

As you might have seen, the customer details are stored in the `ContactInformationUpdated` event on the reservation aggregate. We could simply change this event by adding the new fields, but this would create a problem for all the existing events in the store. If we change the properties in this event, we won't be able to parse all previous events back to the new event.

This is a very important part of event sourcing. You should be able to parse the event stream at all times. It's ok to change events due to new insights, but you must provide a way to parse to old events. Remember that we need all the past events to determine the current state of an aggregate.

Fortunately EventFlow provides a way to deal with this. We'll be using [event upgraders](https://eventflow.readthedocs.io/EventUpgrade.html). We'll keep the first version of the `ContactInformationUpdated` in our code base. Rename it to `ContactInformationUpdatedV1`, and create a new class `ContactInformationUpdated` to represent our current idea on the `ContactInformationUpdated` event. Write an upgrader which can transform a version 1 event to a version 2 event.

**Acceptance criteria**
* `[POST] /Reservation` requires first and last name instead of only name
* `[POST] /Reservation/UpdateContactInformation` requires first and last name instead of only name
* New fields get stored in EventFlow
* Existing reservations can still be hydrated
** You can test it by calling `[POST] /Reservation/UpdateContactInformation` on an existing reservation. 
* Update the readmodel to the new situation. You can write a new migration in the `ReadModelDbMigrator` project
* The e-mail only uses the customers first name.

### How to deal with GDPR
In event sourcing it's not really possible to delete certain events. So whatever you put on the event stream will basically stay there forever. That means that we have to think very carefully about what we store in the event store, and how to deal with "forget me" requests from customers.

As far as we know, there are a few options. And they all have their pro's and cons. In this assignment we'd like you to think about how to handle such a "forget me" request. What options do you have? And why would you choose one over the other? If you'd like, you could make a proof of concept.
