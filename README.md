# SODA.NET

A [Socrata Open Data API](http://dev.socrata.com) (SODA) client library targeting 
.NET 4.5 and above.

## Usage examples

Simple, read-only access

```c#
var client = new SodaClient("data.smgov.net", "AppToken");

//read metadata of a dataset using the resource identifier (Socrata 4x4)
var metadata = client.GetMetadata("1234-wxyz");
Console.WriteLine("Dataset '{0}' has {1} views.", metadata.Name, metadata.ViewsCount);

//get a reference to the resource itself
//the result (a Resouce object) is a generic type
//the type parameter represents the underlying rows of the resource
var dataset = client.GetResource<Dictionary<string, object>>("1234-wxyz");

//of course, a custom type can be used as long as it is JSON serializable
var dataset = client.GetResource<MyClass>("1234-wxyz");

//Resource objects read their own data
var allRows = dataset.GetRows();
var first10Rows = dataset.GetRows(10);

//collections of an arbitrary type can be returned using SoQL and a fluent query building syntax
var soql = new SoqlQuery().Select("column1", "column2").Where("something > nothing").Group("column3");
var results = dataset.Query<MyOtherClass>(soql);
```

**SodaClient** is also capable of performing write operations

```c#
//make sure to provide auth credentials!
var client = new SodaClient("data.smgov.net", "AppToken", "testuser@gmail.com", "password");

//Upsert some data serialized as CSV
string csvData = File.ReadAllText("data.csv");
client.Upsert(csvData, SodaDataFormat.CSV, "1234-wxyz");

//Upsert a collection of serializable entities
IEnumerable<MyClass> payload = GetPayloadData();
client.Upsert(payload, "1234-wxyz");
```

## Dependencies

SODA.NET uses the popular [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) 
library for JSON (de)serialization.

[NUnit 2.6.3](https://www.nuget.org/packages/NUnit/) was used to build the test projects.

NuGet package restore should get you everything you need on first build.

## SODA.Utilities

A set of helper classes and extension methods that we use alongside SODA for our publishing workflow. 
Note, this library is entirely optional and is not required to interact with a Socrata Open Data portal.

SODA.Utilities has a few dependencies of its own:

  - SODA.NET
  - [Microsoft Access Database Engine 2010 Redistributable](http://www.microsoft.com/en-us/download/details.aspx?id=13255)
(for reading Excel documents through OLEDB)
  - [Microsoft Exchange WebServices 2.1](https://www.nuget.org/packages/EWS-Api-2.1/1.0.0) 
(for talking to an Exchange server)

### Usage examples

**SimpleFileLogger**, a simple file logging utility (imagine that!)

```c#
using (var logger = new SimpleFileLogger("log.txt"))
{
    //write a line of text to the log file (and the Console window), e.g.
    //[2014-08-05 14:16:03] Message here
    
    logger.WriteLine("Message here");
}
```

**DataFileExporter**, a utility for exporting data to a text-based file format

```c#
IEnumerable<MyClass> payload = GetPayloadData();

//export as JSON
DataFileExporter.ExportJSON(payload, "data.json");

//export as CSV
DataFileExporter.ExportCSV(payload, "data.csv");
```

**ExcelOleDbHelper**, a utility for reading 
[DataRows](http://msdn.microsoft.com/en-us/library/system.data.datarow) 
out of Excel documents

```c#
//make a connection to an .xls(x) workbook
OleDbConnection connection = ExcelOleDbHelper.MakeConnection("data.xlsx");

//read out a collection of DataRows from all sheets in the workbook
IEnumerable<DataRow> rows = ExcelOleDbHelper.GetRowsFromDataSheets(connection);
```

**IEwsClient**, an interface that wraps some Exchange WebServices functionality

```c#
//initialize a new client targeting Exchange Server 2007 SP1
IEwsClient ewsClient = new Ews2007Sp1Client("username", "password", "domain.org");

//download an attachment (with filename matching the provided regex) from the first unread email
//containing a matching attachment
bool foundAttachment = ewsClient.DownloadAttachment(new Regex("file\\d{6}\\.xlsx"), "C:\\temp");

//send an email message to a list of recipients
ewsClient.SendMessage("Subject Line", "Body text here", "recipient@example.com", "another.recipient@example.com");
```

## Contributing

Check out the 
[Contributor Guidelines](https://github.com/CityOfSantaMonica/SODA.NET/blob/master/CONTRIBUTING.md) 
for more details.

## Copyright and License

Copyright 2014 City of Santa Monica, CA

Licensed under the 
[MIT License](https://github.com/CityOfSantaMonica/SODA.NET/blob/master/LICENSE.txt)