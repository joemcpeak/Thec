using System;
using System.Collections.Generic;
using System.Text;

namespace Thec.Core
{
    // the only trip providers we support.
    // this is baked into Core rather than expecting each trip provider to be in a different assembly that is loaded via reflection based on config, since there
    // is no requirement for that level of flexibility
    public enum TripProvider
    {
        YellowTaxi,
        GreenTaxi,
        ForHireVehicle
    }

    // the NYC boroughs
    public enum Borough
    {
        Bronx,
        Brooklyn,
        EWR,
        Manhattan,
        Queens,
        StatenIsland
    }
}


