using System;
using System.Diagnostics;
using System.Linq;
using InMemoryTests;
using RavenDbTests;
using Shared;
using SisoDbTests;

namespace Benchmarks
{
    class Program
    {
        public static void Main(string[] args)
        {
            var cmd = GetCommand();
            if (cmd != null)
                cmd.Invoke();

            Console.WriteLine("--- Press key to exit ---");
            Console.ReadKey();
        }

        private static Action GetCommand()
        {
            var cmds = new[]
            {
                new { Key = "q", A = null as Action},
                new { Key = "i", A = (Action)RunInMemoryTests },
                new { Key = "s", A = (Action)RunSisoDbTests },
                new { Key = "r", A = (Action)RunRavenDbTests }
            };

            while (true)
            {
                Console.Write("(I)n memory;(S)isoDb;(R)avenDb;(Q)uit;  :>>");
                var cmd = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(cmd))
                    continue;

                var match = cmds.SingleOrDefault(c => c.Key.Equals(cmd, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                    return match.A;
            }
        }

        private static void RunInMemoryTests()
        {
            RunTests(new InMemoryTestCases(), GetTestParams());
        }

        private static void RunSisoDbTests()
        {
            RunTests(new SisoDbTestCases(), GetTestParams());
        }

        private static void RunRavenDbTests()
        {
            RunTests(new RavenDbTestCases(), GetTestParams());
        }

        private static void RunTests<T>(T testCases, TestParams testParams) where T : ITestCases
        {
            var name = typeof (T).Name;
            testCases.Warmup(testParams.CustomerPredicate, testParams.TripPredicate);

            Profile(
                string.Format("{0}-SingleInsertCustomer", name),
                () =>
                {
                    testCases.SingleInsertCustomer();
                    return null;
                },
                testCases.CountCustomers);
            Profile(
                string.Format("{0}-BatchInsertCustomers", name),
                () =>
                {
                    testCases.BatchInsertCustomers(testParams.NumOfCustomers);
                    return null;
                },
                testCases.CountCustomers);
            Profile(
                string.Format("{0}-QueryCustomers", name),
                () => testCases.QueryCustomers(testParams.CustomerPredicate),
                testCases.CountCustomers);
            Profile(
                string.Format("{0}-SingleInsertPackageTrip", name),
                () =>
                {
                    testCases.SingleInsertTrip();
                    return null;
                },
                testCases.CountCustomers);
            Profile(
                string.Format("{0}-BatchInsertPackageTrip", name),
                () =>
                {
                    testCases.BatchInsertTrips(testParams.NumOfCustomers);
                    return null;
                },
                testCases.CountCustomers);
            Profile(
                string.Format("{0}-QueryPackageTrips", name),
                () => testCases.QueryTrips(testParams.TripPredicate),
                testCases.CountCustomers);
        }

        private static TestParams GetTestParams()
        {
            var fns = new[]
            {
                new{Key = "s", Fn = (Func<TestParams>)CreateTestParamsForSmallSet},
                new{Key = "m", Fn = (Func<TestParams>)CreateTestParamsForMediumSet},
                new{Key = "l", Fn = (Func<TestParams>)CreateTestParamsForLargeSet}
            };

            while (true)
            {
                Console.WriteLine("Select size of test data.");
                Console.Write("(S)mall sets (1000); (M)edium sets (10.000); (L)arge sets (100.000);  :>>");
                var ss = Console.ReadLine();
                var match = fns.SingleOrDefault(kv => kv.Key.Equals(ss, StringComparison.OrdinalIgnoreCase));
                if (match == null)
                    continue;

                return match.Fn.Invoke();
            }
        }

        private static TestParams CreateTestParamsForSmallSet()
        {
            const int i = 1;
            const int n = 1000;
            var dateFrom = TestConstants.BaseLine.AddDays(10);
            var dateTo = dateFrom.AddDays(5);

            return new TestParams
            {
                NumOfCustomerItterations = i,
                NumOfCustomers = n,
                CustomerPredicate = c =>
                    c.CustomerNo >= 500 && c.CustomerNo <= 550
                    && c.DeliveryAddress.Zip == "525",

                NumOfTripItterations = i,
                NumOfTrips = n,
                TripPredicate = t =>
                    (t.Transport.DepartureDate >= dateFrom
                    && t.Transport.DepartureDate <= dateTo)
                    && t.Accommodation.Duration > 8
            };
        }

        private static TestParams CreateTestParamsForMediumSet()
        {
            const int i = 1;
            const int n = 10000;
            var dateFrom = TestConstants.BaseLine.AddDays(10);
            var dateTo = dateFrom.AddDays(5);

            return new TestParams
            {
                NumOfCustomerItterations = i,
                NumOfCustomers = n,
                CustomerPredicate = c =>
                    c.CustomerNo >= 5000 && c.CustomerNo <= 5500
                    && c.DeliveryAddress.Zip == "5250",

                NumOfTripItterations = i,
                NumOfTrips = n,
                TripPredicate = t =>
                    (t.Transport.DepartureDate >= dateFrom
                    && t.Transport.DepartureDate <= dateTo)
                    && t.Accommodation.Duration > 8
            };
        }

        private static TestParams CreateTestParamsForLargeSet()
        {
            const int i = 1;
            const int n = 100000;
            var dateFrom = TestConstants.BaseLine.AddDays(10);
            var dateTo = dateFrom.AddDays(5);

            return new TestParams
            {
                NumOfCustomerItterations = i,
                NumOfCustomers = n,
                CustomerPredicate = c =>
                    c.CustomerNo >= 50000 && c.CustomerNo <= 55000
                    && c.DeliveryAddress.Zip == "52500",

                NumOfTripItterations = i,
                NumOfTrips = n,
                TripPredicate = t =>
                    (t.Transport.DepartureDate >= dateFrom
                    && t.Transport.DepartureDate <= dateTo)
                    && t.Accommodation.Duration > 8
            };
        }

        private static void Profile(string title, Func<long?> test, Func<long> counter)
        {
            Console.WriteLine("-------\r\nTESTCASE {0}-------", title);

            for (var i = 0; i < 3; i++)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var r = test.Invoke();
                stopWatch.Stop();

                Console.WriteLine("TotalSeconds = {0}", stopWatch.Elapsed.TotalSeconds);
                if(r.HasValue)
                    Console.WriteLine("Returned rows = {0}", r);
                Console.WriteLine("Total rows = {0}", counter.Invoke());
            }
            Console.WriteLine("-------- -------- --------");
        }
    }
}
