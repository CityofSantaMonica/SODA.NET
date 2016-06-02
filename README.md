# SODA.NET

A [Socrata Open Data API](http://dev.socrata.com) (SODA) client library targeting 
.NET 4.5 and above.

## Getting Started

SODA.NET is available as a [NuGet package](https://www.nuget.org/packages/CSM.SodaDotNet/).

    Install-Package CSM.SodaDotNet

## Usage examples

Simple, read-only access

```c#
//initialize a new client
//make sure you register for your own app token (http://dev.socrata.com/register)
var client = new SodaClient("data.smgov.net", "REPLACE_WITH_YOUR_APP_TOKEN");

//read metadata of a dataset using the resource identifier (Socrata 4x4)
var metadata = client.GetMetadata("1234-wxyz");
Console.WriteLine("{0} has {1} views.", metadata.Name, metadata.ViewsCount);

//get a reference to the resource itself
//the result (a Resouce object) is a generic type
//the type parameter represents the underlying rows of the resource
var dataset = client.GetResource<Dictionary<string, object>>("1234-wxyz");

//of course, a custom type can be used as long as it is JSON serializable
var dataset = client.GetResource<MyClass>("1234-wxyz");

//Resource objects read their own data
var allRows = dataset.GetRows();
var first10Rows = dataset.GetRows(10);

//collections of an arbitrary type can be returned
//using SoQL and a fluent query building syntax
var soql = new SoqlQuery().Select("column1", "column2")
                          .Where("something > nothing")
                          .Group("column3");

var results = dataset.Query<MyOtherClass>(soql);
```

**SodaClient** is also capable of performing write operations

```c#
//make sure to provide auth credentials!
var client = 
    new SodaClient("data.smgov.net", "AppToken", "user@domain.com", "password");

//Upsert some data serialized as CSV
string csvData = File.ReadAllText("data.csv");
client.Upsert(csvData, SodaDataFormat.CSV, "1234-wxyz");

//Upsert a collection of serializable entities
IEnumerable<MyClass> payload = GetPayloadData();
client.Upsert(payload, "1234-wxyz");
```

## Build

Compilation can be done using
[Visual Studio Community Edition](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx).

[NUnit](http://nunit.org/) was used to build and run the test projects. Check out the
[NUnit Test Adapter](https://visualstudiogallery.msdn.microsoft.com/6ab922d0-21c0-4f06-ab5f-4ecd1fe7175d)
to run tests from within Visual Studio.

You can also use the `build.cmd` script, which assumes `msbuild` and `nuget` are available:

    git clone git@github.com:CityofSantaMonica/SODA.NET.git SODA.NET
    cd SODA.NET
    .\build.cmd

To create the Nuget package artifacts, pass an extra parameter:

    .\build.cmd CreatePackages

## Contributing

Check out the 
[Contributor Guidelines](https://github.com/CityOfSantaMonica/SODA.NET/blob/master/CONTRIBUTING.md) 
for more details.

## Copyright and License

Copyright 2016 City of Santa Monica, CA

Licensed under the 
[MIT License](https://github.com/CityOfSantaMonica/SODA.NET/blob/master/LICENSE.txt)

## Thank you

A tremendous amount of inspiration for this project came from the following projects. Thank you!

  - [Octokit.net](https://github.com/octokit/octokit.net)
  - [soda-java](https://github.com/socrata/soda-java/)
  - [soda-dotnet](https://github.com/socrata/soda-dotnet) (defunct?)
