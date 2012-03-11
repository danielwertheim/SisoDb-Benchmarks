using System;
using System.Linq.Expressions;
using Shared;
using Shared.Model;
using SisoDb;
using SisoDb.Sql2012;

namespace SisoDbTests
{
    public class SisoDbTestCases : ITestCases 
    {
        private readonly ISisoDatabase _db;

        public SisoDbTestCases()
        {
            var cnInfo = new Sql2012ConnectionInfo("SisoDb.Sql2012");
            _db = new Sql2012DbFactory().CreateDatabase(cnInfo);
            _db.EnsureNewDatabase();
        }

        public void Warmup(Expression<Func<Customer, bool>> customerPredicate, Expression<Func<Trip, bool>> tripPredicate)
        {
            using(var session = _db.BeginSession())
            {
                var customer = CustomerFactory.CreateCustomers(1)[0];
                session.Insert(customer);
                session.InsertMany(CustomerFactory.CreateCustomers(100));
                var customerById = session.GetById<Customer>(customer.Id);
                var customerByQuery = session.Query<Customer>().Where(customerPredicate).ToArray();
                session.Advanced.DeleteByQuery<Customer>(c => c.CustomerNo > 0);

                var trip = TripFactory.CreateTrips(1)[0];
                session.Insert(trip);
                session.InsertMany(TripFactory.CreateTrips(100));
                var tripById = session.GetById<Trip>(trip.Id);
                var tripByQuery = session.Query<Trip>().Where(tripPredicate).ToArray();
                session.DeleteById<Trip>(trip.Id);
                session.Advanced.DeleteByQuery<Trip>(c => c.Id > 0);
            }
        }

        public void BatchInsertCustomers(int numOfCustomers)
        {
            _db.UseOnceTo().InsertMany(CustomerFactory.CreateCustomers(numOfCustomers));
        }

        public void BatchInsertTrips(int numOfTrips)
        {
            _db.UseOnceTo().InsertMany(TripFactory.CreateTrips(numOfTrips));
        }

        public void SingleInsertCustomer()
        {
            _db.UseOnceTo().Insert(CustomerFactory.CreateCustomers(1)[0]);
        }

        public void SingleInsertTrip()
        {
            _db.UseOnceTo().Insert(TripFactory.CreateTrips(1)[0]);
        }

        public long CountCustomers()
        {
            return _db.UseOnceTo().Count<Customer>();
        }

        public long CountTrips()
        {
            return _db.UseOnceTo().Count<Trip>();
        }

        public long QueryCustomers(Expression<Func<Customer, bool>> predicate)
        {
            return _db.UseOnceTo().Query<Customer>().Where(predicate).ToArray().Length;
        }

        public long QueryTrips(Expression<Func<Trip, bool>> predicate)
        {
            return _db.UseOnceTo().Query<Trip>().Where(predicate).ToArray().Length;
        }
    }
}