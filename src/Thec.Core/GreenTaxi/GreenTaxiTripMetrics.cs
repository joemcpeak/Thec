using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    public class GreenTaxiTripMetrics : TripMetricsBase
    {
        // because we inherit from TripMetricsBase we will get all the "standard" metrics. here are some additional ones
        // we can compute for Green Taxi
        public decimal AveragePassengerCount { get; set; }
        public int MaximumPassengerCount { get; set; }
        public decimal AverageTripDistance { get; set; }
        public decimal MaximumTripDistance { get; set; }
        public decimal AverageTotalAmount { get; set; }
        public decimal MaximumTotalAmount { get; set; }

        public GreenTaxiTripMetrics ()
        {
            this.DrivingService = DrivingService.GreenTaxi;
        }
    }
}
