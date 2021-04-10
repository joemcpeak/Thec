using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public interface ITripMetricsProvider
    {
        ITripMetrics GetMetrics (Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime);
    }
}
