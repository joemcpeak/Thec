using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class TripMetricsProvider
    {
        ITripMetricsProvider _greenTaxiTripMetricsProvider;
        ITripMetricsProvider _yellowTaxiTripMetricsProvider;
        ITripMetricsProvider _forHireVehicleTripMetricsProvider;


        IDrivingServiceDataProvider _drivingServiceDataProvider;
        IConfiguration _configuration;

        public TripMetricsProvider (IDrivingServiceDataProvider drivingServiceDataProvider, IConfiguration configuration)
        {
            _drivingServiceDataProvider = drivingServiceDataProvider;
            _configuration = configuration;
        }

        public TripMetrics GetMetrics (DrivingService drivingService, Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime)
        {
            // decide which provider to use
            ITripMetricsProvider tmp;
            if (drivingService == DrivingService.GreenTaxi)
                tmp = _greenTaxiTripMetricsProvider;
            else
                throw new NotSupportedException($"Trip Provider '{drivingService}' is not supported.");

            // get metrics from the provider
            var tripMetrics = tmp.GetMetrics(startBorough, stopBorough, tripPickupTime);

            // return the metrics to the caller
            return tripMetrics;
        }
    }
}
