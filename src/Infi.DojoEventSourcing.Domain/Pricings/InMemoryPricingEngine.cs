using System;
using System.Collections.Generic;
using LanguageExt;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Pricings
{
    public class InMemoryPricingEngine : IPricingEngine
    {
        private readonly IDictionary<DateTime, Money> _prices = new Dictionary<DateTime, Money>();

        public Option<Money> GetAccommodationPrice(DateTime date) =>
            Option<DateTime>
                .Some(date.Date)
                .Filter(d => _prices.ContainsKey(d))
                .Map(d => _prices[d]);

        public InMemoryPricingEngine SetPriceForDate(DateTime date, Money price)
        {
            _prices[date] = price;
            return this;
        }
    }
}