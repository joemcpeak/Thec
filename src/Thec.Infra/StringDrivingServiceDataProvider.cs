using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thec.Core;

namespace Thec.Infra
{
    public class StringDrivingServiceDataProvider : IDrivingServiceDataProvider
    {
        IConfiguration _configuration;
        string _data;

        public StringDrivingServiceDataProvider (IConfiguration configuration, string data)
        {
            _configuration = configuration;
            _data = data;
        }

        public List<string> GetData (DrivingService drivingService)
        {
            return _data.Split('\n').ToList();
        }
    }
}
