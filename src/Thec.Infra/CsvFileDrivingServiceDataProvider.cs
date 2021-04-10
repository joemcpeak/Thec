using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Thec.Core;

namespace Thec.Infra
{
    public class CsvFileDrivingServiceDataProvider : IDrivingServiceDataProvider
    {

        IConfiguration _configuration;
        string _name;

        public CsvFileDrivingServiceDataProvider (IConfiguration configuration, string name)
        {
            _configuration = configuration;
            _name = name;
        }

        public List<string> GetData ()
        {
            var path = _configuration["CsvFileDrivingServiceDataProvider:FolderPath"];

            var fullName = Path.Combine(path, _name + ".csv");

            List<string> allLines = File.ReadAllLines(fullName).ToList();
            return allLines;
        }
    }
}
