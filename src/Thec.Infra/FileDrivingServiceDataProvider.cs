using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thec.Core;

namespace Thec.Infra
{
    public class FileDrivingServiceDataProvider : IDrivingServiceDataProvider
    {
        IConfiguration _configuration;

        public FileDrivingServiceDataProvider (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> GetData (DrivingService drivingService)
        {
            var path = _configuration[$"{nameof(FileDrivingServiceDataProvider)}:FolderPath"];

            var fullName = Path.Combine(path, drivingService.ToString() + ".csv");

            List<string> allLines = File.ReadAllLines(fullName).ToList();
            return allLines;
        }
    }
}
