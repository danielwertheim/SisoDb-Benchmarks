using System.Collections.Generic;

namespace Shared.Model
{
    public static class TripFactory
    {
        public static IList<Trip> CreateTrips(int numOfTrips)
        {
            var trips = new List<Trip>();
            var dates = new[]
            {
                TestConstants.BaseLine, 
                TestConstants.BaseLine.AddDays(10), 
                TestConstants.BaseLine.AddDays(20),
                TestConstants.BaseLine.AddDays(30),
                TestConstants.BaseLine.AddDays(40)
            };
            var datesIndex = 0;
            var durations = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var durationsIndex = 0;
            var codes = new[] {"02", "12", "22", "32", "42"};
            var codesIndex = 0;

            for (var c = 0; c < numOfTrips; c++)
            {
                trips.Add(new Trip
                {
                    Price = 100 + c,
                    Accommodation = new Accommodation
                    {
                        CheckinDate = dates[datesIndex],
                        HotelCode = string.Concat("H", codes[codesIndex]),
                        RoomCode = "DELUXE",
                        Duration = durations[durationsIndex]
                    },
                    Transport = new Transport
                    {
                        DepartureCode = string.Concat("Dp", codes[codesIndex]),
                        DepartureDate = dates[datesIndex],
                        DestinationCode = string.Concat("Dc", codes[codesIndex]),
                        Duration = durations[durationsIndex]
                    }
                });

                ++datesIndex;
                if (datesIndex == dates.Length - 1)
                    datesIndex = 0;

                ++durationsIndex;
                if (durationsIndex == durations.Length - 1)
                    durationsIndex = 0;

                ++codesIndex;
                if (codesIndex == codes.Length - 1)
                    codesIndex = 0;
            }

            return trips;
        }
    }
}