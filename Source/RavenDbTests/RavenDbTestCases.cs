using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Shared;
using Shared.Model;

namespace RavenDbTests
{
    public class RavenDbTestCases : ITestCases
    {
        private readonly IDocumentStore _store;

        public RavenDbTestCases()
        {
            _store = new DocumentStore { Url = "http://localhost:8080" };
            _store.Initialize();
        }

        public void Warmup(Expression<Func<Customer, bool>> customerPredicate, Expression<Func<Trip, bool>> tripPredicate)
        {
            using (var session = _store.OpenSession())
            {
                var customer = CustomerFactory.CreateCustomers(1)[0];
                session.Store(customer);
                session.SaveChanges();

                var customerById = session.Load<Customer>(customer.Id);
                var customerByQuery = session.Query<Customer>().Where(customerPredicate).ToArray();
                session.Delete(customer);
                session.SaveChanges();

                var trip = TripFactory.CreateTrips(1)[0];
                session.Store(trip);
                session.SaveChanges();

                var tripById = session.Load<Trip>(trip.Id);
                var tripByQuery = session.Query<Trip>().Where(tripPredicate).ToArray();
                session.Delete(trip);
                session.SaveChanges();
            }
        }

        public void BatchInsertCustomers(int numOfCustomers)
        {
            using (var session = _store.OpenSession())
            {
                foreach (var customer in CustomerFactory.CreateCustomers(numOfCustomers))
                    session.Store(customer);
                session.SaveChanges();
            }
        }

        public void BatchInsertTrips(int numOfTrips)
        {
            using (var session = _store.OpenSession())
            {
                foreach (var trip in TripFactory.CreateTrips(numOfTrips))
                    session.Store(trip);
                session.SaveChanges();
            }
        }

        public void SingleInsertCustomer()
        {
            using (var session = _store.OpenSession())
            {
                session.Store(CustomerFactory.CreateCustomers(1)[0]);
                session.SaveChanges();
            }
        }

        public void SingleInsertTrip()
        {
            using (var session = _store.OpenSession())
            {
                session.Store(TripFactory.CreateTrips(1)[0]);
                session.SaveChanges();
            }
        }

        public long CountCustomers()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Customer>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Count();
            }
        }

        public long CountTrips()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Trip>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Count();
            }
        }

        public long QueryCustomers(Expression<Func<Customer, bool>> predicate)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Customer>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Where(predicate)
                    .ToArray().Length;
            }
        }

        public long QueryTrips(Expression<Func<Trip, bool>> predicate)
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Trip>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Where(predicate)
                    .ToArray().Length;
            }
        }
    }
}