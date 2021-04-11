# Introduction

This project is my implementation of the [CSE Take Home Engineering Challenge](https://github.com/seushermsft/Take-Home-Engineering-Challenge) hereafter referred to as &quot;the THEC.&quot;

# High-level Approach

I have implemented the THEC as an ASP.NET Core web API, and I have used Swagger as the user interface to interact with it. The solution is structured as per an Onion architecture.

# Installation Steps

1. Clone the repo locally.
2. Open Thec.sln in Visual Studio 2019.
3. Build the solution to restore the necessary NuGet packages.
4. Unzip file &quot;Thec.zip&quot; from the root of the repo to anywhere on your local machine.
5. In project Thec.Service, edit appsettings.json to specify the location of where you unzipped the data to.
  1. The FileDrivingServiceDataProvider:FolderPath setting must specify just the folder location.
  2. The TaxiZoneLookupProvider:FilePath setting must specify the complete path to file TaxiZoneLoop.csv
  3. Don&#39;t forget to escape all &quot;\&quot; characters.

# Testing via the Swagger UI

1. Start project Thec.Service to launch the web API and navigate to the Swagger page.
2. Give it a try. A few notes:
  1. Only GreenTaxi is supported right now, anything else will generate an exception.
  2. When specifying the trip pickup time, specify a string value in HH:MM military format.
  3. Here is an example that should qualify some information:

![image](https://user-images.githubusercontent.com/47675032/114312718-a6a30800-9ac1-11eb-9c34-48ca2b52cb68.png)

# Running the Tests

You can also execute the automated unit tests to verify proper installation and operation. To do so:

1. Make sure you have built the solution.
2. Open Visual Studio Test Explorer. It will find four tests in project Thec.Tests.
3. Use Test Explorer to execute the tests:

![image](https://user-images.githubusercontent.com/47675032/114312745-b9b5d800-9ac1-11eb-8481-75a4325f1376.png)

# Design Notes

1. I decided to include knowledge of the specific driving services in Thec.Core, rather than abstracting this out. The alternative would have been only to include the interfaces in Core and make each implementer its own assembly that Core could have then loaded dynamically based on the Driving Service specified in the request. I decided to go with the simpler approach since I had no concrete requirement to make it more flexible to support additional driving services.
2. Related to that, in the interest of time I only actually implemented the functionality for Green Taxi although the code is structured so that the other two would follow the same pattern so this should be straightforward.
3. The spec was ambiguous regarding how to determine what historical trips would be relevant to a request. Should it be any trip that was active during the specified pickup time? I finally decided that I would consider a relevant trip to be any trip that started within a configurable window of time from the requested trip pickup time. This window value in minutes is specified as config setting **StartTimeWindowMinutes** in appsettings.json.
4. I wanted to enable each driving service trip metrics provider to be able to provide unique metrics if possible. For instance, For Service Vehicle trips have very little data while the taxi providers have much more. I therefore decided that each TripMetricsProvider would return an ITripMetrics, but each&#39;s implementation could have whatever metrics it wanted. When the returned object is serialized to JSON ASP.NET core will serialize the run-time type and thus all the metrics provided by that driving service provider will be included in the returned JSON. The downside of this approach is that a typed client that is working with the ITripMetricsProvider would have no compile-time way to understand the metrics that a particular request produced. Possible ways to solve this would be to bucket the supplemental metrics as elements in a Dictionary, or use a C# dynamic object. However, since I decided to target usage via Swagger, my approach is probably adequate for that.

# Opportunities for Improvement

1. CSV parsing. Right now the CSV data parsing is very brittle. Each of the three driving service implementations know how to parse their own CSV historical data, and there is no forgiveness. For instance, additional fields in the file will break it, or commas inside of quoted field values. To Do: find an open source CSV parser, or use the one that ships with VB.NET.
2. Historical file reads happen on every request. It would be better to optimize this by reading the files either on initialization or lazily, but regardless caching the parse file info locally so that it only needs to happen once.
3. As discussed previously, I only implemented the GreenTaxisTripMetricsProvider in the interest of time. The Yellow Taxi and For Hire Vehicles would follow the same pattern, but this pattern would dictate some repetition of common code (for example, taxi zone lookup). This could perhaps be refactored into a base class TripMetricsProviderBase that holds the common functionality and from which each driving service specific trip metrics provider would derive.
4. Some additional tests should be added. For instance, to test proper rounding, etc. when the averaging of values across relevant trips is not neat. I am just using the LINQ Average() operator though so I would expect that this would calculate properly with such data. Also, I wasn&#39;t sure how I wanted to create tests for the Thec.Infra.FileDrivingServiceDataProvider class. One way I have done this is the past is to just include sample files in the test project and point the component to those via config. However because these files are so big I didn&#39;t want to include them as part of the project and have to deal with them in Git. I did do a variation on this by creating a trimmed version of GreenTaxi.csv in the Thec.Tests project&#39;s CsvData folder. Test TestWithFileData uses a FileDrivingServiceDataProvider to read this trimmed version.
