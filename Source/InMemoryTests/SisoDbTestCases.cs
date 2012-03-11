using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Shared;
using Shared.Model;

namespace InMemoryTests
{
    /// <summary>
    /// Only used to see that queries returns correct numbers.
    /// </summary>
    public class InMemoryTestCases : ITestCases
    {
        private readonly List<Customer> _customers;
        private readonly List<Trip> _trips;

        public InMemoryTestCases()
        {
            _customers = new List<Customer>();
            _trips = new List<Trip>();
        }

        public void Warmup(Expression<Func<Customer, bool>> customerPredicate, Expression<Func<Trip, bool>> tripPredicate)
        {
        }

        public void BatchInsertCustomers(int numOfCustomers)
        {
            _customers.AddRange(CustomerFactory.CreateCustomers(numOfCustomers));
        }

        public void BatchInsertTrips(int numOfTrips)
        {
            _trips.AddRange(TripFactory.CreateTrips(numOfTrips));
        }

        public void SingleInsertCustomer()
        {
            _customers.Add(CustomerFactory.CreateCustomers(1)[0]);
        }

        public void SingleInsertTrip()
        {
            _trips.Add(TripFactory.CreateTrips(1)[0]);
        }

        public long CountCustomers()
        {
            return _customers.Count;
        }

        public long CountTrips()
        {
            return _trips.Count;
        }

        public long QueryCustomers(Expression<Func<Customer, bool>> predicate)
        {
            return _customers.Where(predicate.Compile()).ToArray().Length;
        }

        public long QueryTrips(Expression<Func<Trip, bool>> predicate)
        {
            return _trips.Where(predicate.Compile()).ToArray().Length;
        }
    }
}