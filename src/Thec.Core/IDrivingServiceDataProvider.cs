using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thec.Core
{
    public interface IDrivingServiceDataProvider
    {
        // for now we will give back a List<string> but in the future maybe just give back a stream for more flexibility (e.g. reading from 
        // HTTP or blog storage)
        List<string> GetData ();
    }
}
