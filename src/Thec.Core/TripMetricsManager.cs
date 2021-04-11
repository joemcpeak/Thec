using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class TripMetricsManager : ITripMetricsManager
    {
        private readonly IConfiguration _configuration;
        private readonly IDrivingServiceDataProvider _drivingServiceDataProvider;

        public TripMetricsManager (IConfiguration configuration, IDrivingServiceDataProvider drivingServiceDataProvider)
        {
            _configuration = configuration;
            _drivingServiceDataProvider = drivingServiceDataProvider;
        }

        public ITripMetrics GetMetrics (DrivingService drivingService, Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime)
        {
            // decide which provider to use
            ITripMetricsProvider tmp;
            if (drivingService == DrivingService.GreenTaxi)
                tmp = new GreenTaxiTripMetricsProvider(_configuration, _drivingServiceDataProvider);
            else
                throw new NotSupportedException($"Trip Provider '{drivingService}' is not yet supported.");

            // get metrics from the provider
            var tripMetrics = tmp.GetMetrics(startBorough, stopBorough, tripPickupTime);

            // return the metrics to the caller
            return tripMetrics;
        }
    }
}
