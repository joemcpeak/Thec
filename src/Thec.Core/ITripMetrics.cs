using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public interface ITripMetrics
    {
        DrivingService DrivingService { get; set; }
        Borough StartBorough { get; set; }
        Borough StopBorough { get; set; }
        TimeSpan TripTime { get; set; }
        TimeSpan AverageTripDuration { get; set; }
        int TripCount { get; set; }
    }
}
