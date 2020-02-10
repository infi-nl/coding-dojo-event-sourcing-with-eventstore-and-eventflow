using System;

namespace Infi.DojoEventSourcing.Domain.Rooms
{
    public class NoRoomsAvailableException : Exception
    {
        public NoRoomsAvailableException(string message)
            : base(message)
        {
        }
    }
}