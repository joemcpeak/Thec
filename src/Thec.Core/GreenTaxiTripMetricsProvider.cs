using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class GreenTaxiTripMetricsProvider : ITripMetricsProvider
    {

        public GreenTaxiTripMetricsProvider (IDrivingServiceDataProvider drivingServiceDataProvider)
        {
            
        }


        public TripMetrics GetMetrics (Borough startBorough, Borough borough, TimeSpan tripPickupTime)
        {
            return new TripMetrics();
        }
    }
}
