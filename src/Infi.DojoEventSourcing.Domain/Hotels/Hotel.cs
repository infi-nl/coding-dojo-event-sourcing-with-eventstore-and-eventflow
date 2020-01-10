using System;
using NodaMoney;

namespace Infi.DojoEventSourcing.Domain.Hotels
{
    public class Hotel
    {
        public static Currency Currency => Currency.FromCode("EUR");

        public static TimeZoneInfo Timezone => TimeZoneInfo.FindSystemTimeZoneById("Europe/Amsterdam");

        public static DateTime CheckInTime = new DateTime(0, 0, 0, 14, 0, 0);
        public static DateTime CheckOutTime = new DateTime(0, 0, 0, 12, 0, 0);

        public static DateTime CreateCheckInTimeFromDate(DateTime arrival) =>
            TimeZoneInfo.ConvertTimeFromUtc((arrival.Date + CheckInTime.TimeOfDay).ToUniversalTime(), Timezone);

        public static DateTime CreateCheckOutTimeFromDate(DateTime departure) =>
            TimeZoneInfo.ConvertTimeFromUtc((departure + CheckOutTime.TimeOfDay).ToUniversalTime(), Timezone);
    }
}