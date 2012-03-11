using System;

namespace Shared.Model
{
    public class Accommodation
    {
        public string HotelCode { get; set; }
        public string RoomCode { get; set; }
        public DateTime CheckinDate { get; set; }
        public int Duration { get; set; }
    }
}