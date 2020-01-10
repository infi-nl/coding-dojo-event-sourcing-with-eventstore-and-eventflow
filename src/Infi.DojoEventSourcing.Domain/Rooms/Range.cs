using System;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class Range
    {
        public Range(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                throw new ArgumentException($"Start must be before end, but was: start {start}, {end}");
            }

            Start = start;
            End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public bool Overlaps(Range withRange) => Start < withRange.End && End > withRange.Start;
    }
}