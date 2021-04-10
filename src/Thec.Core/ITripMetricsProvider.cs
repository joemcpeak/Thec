using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public interface ITripMetricsProvider
    {
        TripMetrics GetMetrics (Borough startBorough, Borough borough, TimeSpan tripPickupTime);
    }
}
