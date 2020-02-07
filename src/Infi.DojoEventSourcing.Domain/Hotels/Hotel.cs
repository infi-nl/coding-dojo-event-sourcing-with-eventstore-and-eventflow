using System;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Hotels
{
    public class Hotel
    {
        public static Currency Currency => Currency.FromCode("EUR");

        public static TimeZoneInfo Timezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");

        private static readonly TimeSpan CheckInTime = TimeSpan.FromHours(14);
        private static readonly TimeSpan CheckOutTime = TimeSpan.FromHours(12);

        public static DateTime CreateCheckInTimeFromDate(DateTime arrival) =>
            TimeZoneInfo.ConvertTimeFromUtc((arrival.Date + CheckInTime).ToUniversalTime(), Timezone);

        public static DateTime CreateCheckOutTimeFromDate(DateTime departure) =>
            TimeZoneInfo.ConvertTimeFromUtc((departure.Date + CheckOutTime).ToUniversalTime(), Timezone);
    }
}