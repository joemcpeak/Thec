using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thec.Core
{

    public class TaxiZoneLookupProvider
    {
        IConfiguration _configuration;
        Dictionary<int, Borough> _taxiZones;

        public TaxiZoneLookupProvider (IConfiguration configuration)
        {
            _configuration = configuration;

            // get the taxi zones
            _taxiZones = GetTaxiZones();
        }

        private Dictionary<int, Borough> GetTaxiZones ()
        {
            // this method gets the reference data for the borough associated with each location id

            // we assume that this reference data is always a local CSV file we will read in
            var path = _configuration[$"{nameof(TaxiZoneLookupProvider)}:FilePath"];
            var lines = File.ReadAllLines(path);

            var taxiZones = new Dictionary<int, Borough>();

            // parse through each line to extract the location id and the associated borough
            // (skip line 0 of course - the headings)

            for (int i = 1; i < lines.Length; i++)
            {
                var fields = lines[i].Split(',');
                // strip off double quotes and collapse spaces (thank you, Staten Island) before converting the string to teh Borough enum value
                var borough = (Borough)Enum.Parse(typeof(Borough), fields[1].Replace("\"", "").Replace(" ", ""));
                taxiZones.Add(int.Parse(fields[0]), borough);
            }

            // now we have a lookup dictionary that for every location key returns its associated borough
            return taxiZones;
        }

        public Borough GetBoroughForTaxiZone (int locationID)
        {
            return _taxiZones[locationID];
        }

    }
}
