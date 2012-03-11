using System;

namespace Shared.Model
{
    public class Transport
    {
        public string DepartureCode { get; set; }
        public string DestinationCode { get; set; }
        public DateTime DepartureDate { get; set; }
        public int Duration { get; set; }
    }
}