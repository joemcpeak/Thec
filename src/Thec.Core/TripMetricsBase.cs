using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class TripMetricsBase : ITripMetrics
    {
        public DrivingService DrivingService { get; set; }
        public Borough StartBorough { get; set; }
        public Borough StopBorough { get; set; }
        public TimeSpan TripTime { get; set; }
        public TimeSpan AverageTripDuration { get; set; }
        public int TripCount { get; set; }
    }
}
