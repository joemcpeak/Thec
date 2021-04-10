using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    interface ITripMetricsProvider
    {
        TripMetrics GetMetrics (TripProvider tripProvider, Borough startBorough, Borough borough, TimeSpan tripPickupTime);
    }
}
