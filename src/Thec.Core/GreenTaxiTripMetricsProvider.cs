using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

        public TripMetrics GetMetrics (Borough startBorough, Borough borough, TimeSpan tripPickupTime)
        {
            return new TripMetrics();
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
                // split the line apart
                var fields = line.Split(new char[] { ',' });

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
