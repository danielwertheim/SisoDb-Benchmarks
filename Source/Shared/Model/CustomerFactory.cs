using System.Collections.Generic;

namespace Shared.Model
{
    public static class CustomerFactory
    {
        private static int _itteratorCount;

        public static IList<Customer> CreateCustomers(int numOfCustomers)
        {
            var customers = new List<Customer>();
            var dates = new[]
            {
                TestConstants.BaseLine.AddDays(10), 
                TestConstants.BaseLine.AddDays(20), 
                TestConstants.BaseLine.AddDays(30),
                TestConstants.BaseLine.AddDays(40),
                TestConstants.BaseLine.AddDays(50)
            };
            var datesIndex = 0;

            for (var c = 0; c < numOfCustomers; c++)
            {
                var n = _itteratorCount + (c + 1);
                customers.Add(new Customer
                {
                    CustomerNo = c,
                    Firstname = "Daniel",
                    Lastname = "Wertheim",
                    ShoppingIndex = ShoppingIndexes.Level1,
                    CustomerSince = dates[datesIndex],
                    BillingAddress =
                        {
                            Street = "The billing street " + n,
                            Zip = c.ToString(),
                            City = "The billing City",
                            Country = "Sweden-billing",
                            AreaCode = 1000 + n
                        },
                    DeliveryAddress =
                        {
                            Street = "The delivery street #" + n,
                            Zip = c.ToString(),
                            City = "The delivery City",
                            Country = "Sweden-delivery",
                            AreaCode = -1000 - n
                        }
                });

                ++datesIndex;
                if (datesIndex == dates.Length - 1)
                    datesIndex = 0;
            }

            _itteratorCount += numOfCustomers;

            return customers;
        }
    }
}