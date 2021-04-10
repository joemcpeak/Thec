using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Thec.Core;
using Thec.Infra;

namespace Thec.Core.Tests
{
    [TestClass]
    public class GreenTaxiTests
    {

        private readonly IConfigurationRoot _configuration;

        public GreenTaxiTests ()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            _configuration = config.Build();
        }

        [TestMethod]
        public void TestGreenTaxi1 ()
        {
            var drivingServices = DrivingService.GreenTaxi;
            var startBorough = Borough.Manhattan;
            var stopBorough = Borough.Manhattan;
            var tripStartTime = TimeSpan.Parse("00:30");

            var dataProvider = new FileDrivingServiceDataProvider(_configuration, drivingServices.ToString());
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime);
        }

        [TestMethod]
        public void TestNoRelevantTrips ()
        {
            var drivingServices = DrivingService.GreenTaxi;
            var startBorough = Borough.Manhattan;
            var stopBorough = Borough.Manhattan;
            var tripStartTime = TimeSpan.Parse("00:30");

            // both of these trips started in Queens
            var data =
@"VendorID,lpep_pickup_datetime,lpep_dropoff_datetime,store_and_fwd_flag,RatecodeID,PULocationID,DOLocationID,passenger_count,trip_distance,fare_amount,extra,mta_tax,tip_amount,tolls_amount,ehail_fee,improvement_surcharge,total_amount,payment_type,trip_type
2,2018-01-01 00:18:50,2018-01-01 00:24:39,N,1,19,236,5,.70,6,0.5,0.5,0,0,,0.3,7.3,2,1
2,2018-01-01 00:30:26,2018-01-01 00:46:42,N,1,19,42,5,3.50,14.5,0.5,0.5,0,0,,0.3,15.8,2,1
";
            var dataProvider = new StringDrivingServiceDataProvider(_configuration, data);
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime);

            Assert.IsNull(metrics);
        }

        [TestMethod]
        public void TestOneRelevantTrip ()
        {
            var drivingServices = DrivingService.GreenTaxi;
            var startBorough = Borough.Brooklyn;
            var stopBorough = Borough.Manhattan;
            var tripStartTime = TimeSpan.Parse("00:30");

            // the first trip starts in brooklyn and ends in manhattan and is the one that should qualify as relevant
            var data =
@"VendorID,lpep_pickup_datetime,lpep_dropoff_datetime,store_and_fwd_flag,RatecodeID,PULocationID,DOLocationID,passenger_count,trip_distance,fare_amount,extra,mta_tax,tip_amount,tolls_amount,ehail_fee,improvement_surcharge,total_amount,payment_type,trip_type
2,2018-01-01 00:15:00,2018-01-01 00:24:39,N,1,255,236,5,.70,6,0.5,0.5,0,0,,0.3,7.3,2,1
2,2018-01-01 00:30:26,2018-01-01 00:46:42,N,1,19,42,5,3.50,14.5,0.5,0.5,0,0,,0.3,15.8,2,1
";
            var dataProvider = new StringDrivingServiceDataProvider(_configuration, data);
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime) as GreenTaxiTripMetrics;

            // since only one trip shoudl qualify, all these metrics shoudl exactly equal the values for that single trip
            Assert.AreEqual(5, metrics.AveragePassengerCount);
            Assert.AreEqual(7.3m, metrics.AverageTotalAmount);
            Assert.AreEqual(0.70m, metrics.AverageTripDistance);
            Assert.AreEqual(new TimeSpan(0, 9, 39), metrics.AverageTripDuration);
            Assert.AreEqual(DrivingService.GreenTaxi, metrics.DrivingService);
            Assert.AreEqual(5, metrics.MaximumPassengerCount);
            Assert.AreEqual(7.3m, metrics.MaximumTotalAmount);
            Assert.AreEqual(0.70m, metrics.MaximumTripDistance);
            Assert.AreEqual(Borough.Brooklyn, metrics.StartBorough);
            Assert.AreEqual(Borough.Manhattan, metrics.StopBorough);
            Assert.AreEqual(1, metrics.TripCount);
            Assert.AreEqual(tripStartTime, metrics.TripTime);
        }

        [TestMethod]
        public void TestTwoRelevantTrips ()
        {
            var drivingServices = DrivingService.GreenTaxi;
            var startBorough = Borough.Brooklyn;
            var stopBorough = Borough.Manhattan;
            var tripStartTime = TimeSpan.Parse("00:30");

            // the 2nd and 3rd trips shoudl be relevant
            var data =
@"VendorID,lpep_pickup_datetime,lpep_dropoff_datetime,store_and_fwd_flag,RatecodeID,PULocationID,DOLocationID,passenger_count,trip_distance,fare_amount,extra,mta_tax,tip_amount,tolls_amount,ehail_fee,improvement_surcharge,total_amount,payment_type,trip_type
2,2018-01-01 00:30:26,2018-01-01 00:46:42,N,1,19,42,5,3.50,14.5,0.5,0.5,0,0,,0.3,15.8,2,1
2,2018-01-01 00:15:00,2018-01-01 00:16:00,N,1,255,236,2,1,6,0.5,0.5,0,0,,0.3,10,2,1
2,2018-01-01 00:18:00,2018-01-01 00:20:00,N,1,255,236,4,2.5,6,0.5,0.5,0,0,,0.3,15,2,1
2,2018-01-01 00:30:26,2018-01-01 00:46:42,N,1,19,42,5,3.50,14.5,0.5,0.5,0,0,,0.3,15.8,2,1
";
            var dataProvider = new StringDrivingServiceDataProvider(_configuration, data);
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime) as GreenTaxiTripMetrics;

            // since only one trip should qualify, all these metrics should exactly equal the values for that single trip
            Assert.AreEqual(3, metrics.AveragePassengerCount);
            Assert.AreEqual(12.50m, metrics.AverageTotalAmount);
            Assert.AreEqual(1.75m, metrics.AverageTripDistance);
            Assert.AreEqual(new TimeSpan(0, 1, 30), metrics.AverageTripDuration);
            Assert.AreEqual(DrivingService.GreenTaxi, metrics.DrivingService);
            Assert.AreEqual(4, metrics.MaximumPassengerCount);
            Assert.AreEqual(15m, metrics.MaximumTotalAmount);
            Assert.AreEqual(2.5m, metrics.MaximumTripDistance);
            Assert.AreEqual(Borough.Brooklyn, metrics.StartBorough);
            Assert.AreEqual(Borough.Manhattan, metrics.StopBorough);
            Assert.AreEqual(2, metrics.TripCount);
            Assert.AreEqual(tripStartTime, metrics.TripTime);
        }
    }
}
