using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Thec.Core
{
    public class GreenTaxiTripMetricsProvider : ITripMetricsProvider
    {
        IConfiguration _configuration;
        IDrivingServiceDataProvider _drivingServiceDataProvider;
        TaxiZoneLookupProvider _taxiZoneLookupProvider;
        List<GreenTaxiTrip> _trips;

        public GreenTaxiTripMetricsProvider (IConfiguration configuration, IDrivingServiceDataProvider drivingServiceDataProvider)
        {
            _configuration = configuration;
            _drivingServiceDataProvider = drivingServiceDataProvider;

            _taxiZoneLookupProvider = new TaxiZoneLookupProvider(_configuration);

            // get and parse the historical trip data
            var data = _drivingServiceDataProvider.GetData();
            _trips = ParseHistoricalCsvData(data);
        }

        public ITripMetrics GetMetrics (Borough startBorough, Borough stopBorough, TimeSpan tripPickupTime)
        {
            var windowMinutes = int.Parse(_configuration["StartTimeWindowMinutes"]);

            TimeSpan startTime = tripPickupTime - new TimeSpan(0, windowMinutes, 0);
            TimeSpan stopTime = tripPickupTime + new TimeSpan(0, windowMinutes, 0);

            // find the historical trips relevant to this request
            var relevantTrips = _trips.Where(t => t.StartBorough == startBorough &&
                                                  t.StopBorough == stopBorough &&
                                                  (t.PickupDateTime.TimeOfDay >= startTime && t.PickupDateTime.TimeOfDay <= stopTime)
                                             );

            // if no trips were relevant, we can't return any metrics
            if (!relevantTrips.Any())
                return null;

            // otherwise let's compute some metrics!
            var metrics = new GreenTaxiTripMetrics();
            metrics.TripCount = relevantTrips.Count();
            metrics.StartBorough = startBorough;
            metrics.StopBorough = stopBorough;
            metrics.TripTime = tripPickupTime;
            metrics.AverageTripDuration = TimeSpan.FromSeconds((relevantTrips.Average(t => (t.DropoffDateTime - t.PickupDateTime).TotalSeconds)));
            metrics.AveragePassengerCount = Convert.ToDecimal(relevantTrips.Average(t => t.PassengerCount));
            metrics.MaximumPassengerCount = relevantTrips.Max(t => t.PassengerCount);
            metrics.AverageTripDistance = Convert.ToDecimal(relevantTrips.Average(t => t.TripDistance));
            metrics.MaximumTripDistance = Convert.ToDecimal(relevantTrips.Max(t => t.TripDistance));
            metrics.AverageTotalAmount = Convert.ToDecimal(relevantTrips.Average(t => t.TotalAmount));
            metrics.MaximumTotalAmount = Convert.ToDecimal(relevantTrips.Average(t => t.TotalAmount));

            // return the completed metrics
            return metrics;
        }

        private List<GreenTaxiTrip> ParseHistoricalCsvData (List<string> data)
        {
            // each record of the csv should have this many fields
            var expectedFieldCount = 19;

            // what we will build
            var trips = new List<GreenTaxiTrip>();

            // remove the first record, which will be the headings
            data.RemoveAt(0);

            // parse each line
            foreach (var line in data)
            {
                // skip blank lines
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // split the line apart
                var fields = line.Split(',');

                // make sure the line has the number of fields we expect
                if (fields.Length != expectedFieldCount)
                    throw new Exception("Unexpected count!");

                // build the new base record
                var trip = new GreenTaxiTrip();
                trip.VendorId = int.Parse(fields[0]);
                trip.PickupDateTime = DateTime.Parse(fields[1]);
                trip.DropoffDateTime = DateTime.Parse(fields[2]);
                trip.StoreAndForwardFlag = fields[3];
                trip.RateCodeId = int.Parse(fields[4]);
                trip.PickupLocationID = int.Parse(fields[5]);
                trip.DropoffLocatioId = int.Parse(fields[6]);
                trip.PassengerCount = int.Parse(fields[7]);
                trip.TripDistance = decimal.Parse(fields[8]);
                trip.FareAount = decimal.Parse(fields[9]);
                trip.Extra = decimal.Parse(fields[10]);
                trip.MtaTax = decimal.Parse(fields[11]);
                trip.TipAmount = decimal.Parse(fields[12]);
                trip.TollsAmount = decimal.Parse(fields[13]);
                trip.EHailFee = string.IsNullOrWhiteSpace(fields[14]) ? 0 : decimal.Parse(fields[14]);
                trip.ImprovementSurcharge = decimal.Parse(fields[15]);
                trip.TotalAmount = decimal.Parse(fields[16]);
                trip.PaymentType = int.Parse(fields[17]);
                trip.TripType = int.Parse(fields[18]);

                // derive some additional info
                trip.StartBorough = _taxiZoneLookupProvider.GetBoroughForTaxiZone(trip.PickupLocationID);
                trip.StopBorough = _taxiZoneLookupProvider.GetBoroughForTaxiZone(trip.DropoffLocatioId);

                // add the record to our list
                trips.Add(trip);
            }

            // return the final list we built
            return trips;
        }
    }
}
