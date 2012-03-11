using System;
using System.Linq.Expressions;
using Shared.Model;

namespace Benchmarks
{
    public class TestParams
    {
        public int NumOfCustomers;
        public int NumOfCustomerItterations;

        public int NumOfTrips;
        public int NumOfTripItterations;

        public Expression<Func<Customer, bool>> CustomerPredicate;
        public Expression<Func<Trip, bool>> TripPredicate;
    }
}