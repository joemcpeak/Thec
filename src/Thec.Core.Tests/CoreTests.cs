using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Thec.Core;
using Thec.Infra;

namespace Thec.Core.Tests
{
    [TestClass]
    public class CoreTests
    {

        private readonly IConfigurationRoot _configuration;

        public CoreTests ()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            _configuration = config.Build();
        }

        [TestMethod]
        public void TestGreenTaxi1 ()
        {
            var drivingServices = DrivingService.GreenTaxi;
            var startBorough = Borough.Brooklyn;
            var stopBorough = Borough.Queens;
            var tripStartTime = TimeSpan.Parse("15:20");

            var dataProvider = new FileDrivingServiceDataProvider(_configuration, drivingServices.ToString());
            var tripMetricsProvider = new TripMetricsProvider(_configuration, dataProvider);

            var metrics = tripMetricsProvider.GetMetrics(drivingServices, startBorough, stopBorough, tripStartTime);
        }
    }
}
