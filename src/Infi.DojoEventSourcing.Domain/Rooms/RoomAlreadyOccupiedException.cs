using System;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class RoomAlreadyOccupiedException : Exception
    {
        public RoomAlreadyOccupiedException(string message) : base(message)
        {
        }
    }
}