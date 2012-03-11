using System;
using System.Linq.Expressions;
using Shared.Model;

namespace Shared
{
    public interface ITestCases
    {
        void Warmup(Expression<Func<Customer, bool>> customerPredicate, Expression<Func<Trip, bool>> tripPredicate);

        void BatchInsertCustomers(int numOfCustomers);
        void BatchInsertTrips(int numOfTrips);
        void SingleInsertCustomer();
        void SingleInsertTrip();

        long QueryCustomers(Expression<Func<Customer, bool>> predicate);
        long QueryTrips(Expression<Func<Trip, bool>> predicate);

        long CountCustomers();
        long CountTrips();
    }
}