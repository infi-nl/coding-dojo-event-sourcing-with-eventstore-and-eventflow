using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using Infi.DojoEventSourcing.Domain.Hotels;
using Infi.DojoEventSourcing.Domain.Pricings;
using Infi.DojoEventSourcing.Domain.Reservations.Events;
using Infi.DojoEventSourcing.Domain.Reservations.ValueObjects;
using Infi.DojoEventSourcing.Domain.Rooms;

namespace Infi.DojoEventSourcing.Domain.Reservations
{
    public class Reservation
        : AggregateRoot<Reservation, ReservationId>,
            IEmit<ContactInformationUpdated>,
            IEmit<LineItemCreated>,
            IEmit<PriceOffered>,
            IEmit<ReservationCreated>
    {
        private static readonly TimeSpan PriceValidityDuration = TimeSpan.FromMinutes(30);

        private readonly IDictionary<DateTime, PriceOffered> _priceOffersByDate =
            new Dictionary<DateTime, PriceOffered>();

        private State _state = State.Prospective;
        private int _lineItems = 0;

        public enum State
        {
            Prospective,
            Reserved,
        }

        public Reservation(ReservationId id)
            : base(id)
        {
        }

        public void CreateOffers(DateTime arrival, DateTime departure, IPricingEngine pricing)
        {
            for (var date = arrival; date < departure; date = date.AddDays(1))
            {
                MakePriceOffer(date, pricing);
            }
        }

        private void MakePriceOffer(DateTime date, IPricingEngine pricing)
        {
            if (HasValidPriceOffer(date))
            {
                return;
            }

            pricing
                .GetAccommodationPrice(date)
                .IfSome(price =>
                {
                    var expires = DateTime.UtcNow + PriceValidityDuration;
                    Emit(new PriceOffered(Id, date, price, expires));
                });
        }

        private bool HasValidPriceOffer(DateTime date) =>
            _priceOffersByDate.ContainsKey(date) && _priceOffersByDate[date].IsStillValid();

        public void UpdateContactInformation(string name, string email)
        {
            Emit(new ContactInformationUpdated(Id, name, email));
        }

        public void MakeReservation(DateTime arrival, DateTime departure)
        {
            CheckStateIs(State.Prospective);
            Emit(new ReservationCreated(
                Id,
                arrival,
                departure,
                Hotel.CreateCheckInTimeFromDate(arrival),
                Hotel.CreateCheckOutTimeFromDate(departure)));

            for (var date = arrival; date < departure; date = date.AddDays(1))
            {
                var offer = GetValidPriceOffer(date);
                Emit(new LineItemCreated(Id, _lineItems + 1, offer.Date, offer.Price));
            }
        }

        private PriceOffered GetValidPriceOffer(DateTime date)
        {
            if (!_priceOffersByDate.ContainsKey(date))
            {
                throw new ArgumentException("no price offer for date " + date);
            }

            var offer = _priceOffersByDate[date];
            if (offer.HasExpired())
            {
                throw new ArgumentException("price offer for date " + date + " has expired");
            }

            return offer;
        }

        public void AssignRoom(Room.RoomIdentity roomId, string roomNumber)
        {
            CheckStateIs(State.Reserved);
            Emit(new RoomAssigned(Id, roomId, roomNumber));
        }

        private void CheckStateIs(State expected)
        {
            if (_state != expected)
            {
                throw new ArgumentException("unexpected state: " + _state);
            }
        }

        public void Apply(ReservationCreated aggregateEvent)
        {
            // FIXME Implement
            // throw new NotImplementedException();
        }

        public void Apply(ContactInformationUpdated aggregateEvent)
        {
            // FIXME Implement
            // throw new NotImplementedException();
        }

        public void Apply(PriceOffered priceOffered)
        {
            // FIXME Add logging
            if (_priceOffersByDate.ContainsKey(priceOffered.Date))
            {
                _priceOffersByDate.Remove(priceOffered.Date);
            }

            _priceOffersByDate.Add(priceOffered.Date, priceOffered);
        }

        public void Apply(LineItemCreated aggregateEvent)
        {
            // FIXME Implement
        }
    }
}