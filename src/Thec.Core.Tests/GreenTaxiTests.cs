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

            // the first trip starts in brooklyn and ends in manhattan
            var data =
@"VendorID,lpep_pickup_datetime,lpep_dropoff_datetime,store_and_fwd_flag,RatecodeID,PULocationID,DOLocationID,passenger_count,trip_distance,fare_amount,extra,mta_tax,tip_amount,tolls_amount,ehail_fee,improvement_surcharge,total_amount,payment_type,trip_type
2,2018-01-01 00:18:50,2018-01-01 00:24:39,N,1,255,236,5,.70,6,0.5,0.5,0,0,,0.3,7.3,2,1
2,2018-01-01 00:30:26,2018-01-01 00:46:42,N,1,19,42,5,3.50,14.5,0.5,0.5,0,0,,0.3,15.8,2,1
";
            var dataProvider = new StringDrivingServiceDataProvider(_configuration, data);
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime);

            var foo = (ITripMetrics)metrics;

        }

    }
}
