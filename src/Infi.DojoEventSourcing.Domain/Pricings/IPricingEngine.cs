using System;
using LanguageExt;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Pricings
{
    public interface IPricingEngine
    {
        Option<Money> GetAccommodationPrice(DateTime date);
    }
}