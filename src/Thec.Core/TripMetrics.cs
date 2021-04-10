using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{

    public class TripMetrics
    {
        public DrivingService TripProvider { get; set; }
        public Borough StartBorough { get; set; }
        public Borough StopBorough { get; set; }
        public TimeSpan TripTime { get; set; }
        public TimeSpan AverageTripDuration { get; set; }
        public int AverageTripCount { get; set; }

        // todo - figure out how specific trip providers can supplement this with metrics they can add, since each T.P. has additional info they can surface
    }
}
