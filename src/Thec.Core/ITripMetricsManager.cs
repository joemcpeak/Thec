using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public interface ITripMetricsManager
    {
        ITripMetrics GetMetrics (DrivingService drivingService, Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime);
    }
}
