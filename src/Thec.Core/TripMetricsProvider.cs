using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class TripMetricsProvider
    {
        IConfiguration _configuration;
        IDrivingServiceDataProvider _drivingServiceDataProvider;

        public TripMetricsProvider (IConfiguration configuration, IDrivingServiceDataProvider drivingServiceDataProvider)
        {
            _configuration = configuration;
            _drivingServiceDataProvider = drivingServiceDataProvider;
        }

        public TripMetrics GetMetrics (DrivingService drivingService, Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime)
        {
            // decide which provider to use
            ITripMetricsProvider tmp;
            if (drivingService == DrivingService.GreenTaxi)
                tmp = new GreenTaxiTripMetricsProvider(_configuration, _drivingServiceDataProvider);
            else
                throw new NotSupportedException($"Trip Provider '{drivingService}' is not supported.");

            // get metrics from the provider
            var tripMetrics = tmp.GetMetrics(startBorough, stopBorough, tripPickupTime);

            // return the metrics to the caller
            return tripMetrics;
        }
    }
}
