using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thec.Core
{
    public interface IDrivingServiceDataProvider
    {
        List<string> GetData ();
    }
}
