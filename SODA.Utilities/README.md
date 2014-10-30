# SODA.Utilities

A set of helper classes and extension methods that we use alongside SODA.NET for our publishing workflow. 
Note, this library is entirely optional and is not required to interact with a Socrata Open Data portal.

## Usage examples

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

//regex to match against attachment filenames
var regx = new Regex("file\\d{6}\\.xlsx");

//download an attachment
//from the first unread email containing a matching attachment
bool foundAttachment = ewsClient.DownloadAttachment(regx, "C:\\temp");

//send an email message to a list of recipients
ewsClient.SendMessage("Subject Line",
                      "Body text here",
                      "recipient@example.com",
                      "another.recipient@example.com");
```

**SeparatedValuesSerializer**, a utility for serializing a collection to a "separated values" (e.g. CSV) representation
```c#
IEnumerable<MyClass> payload = GetPayloadData();

string payloadCSV = 
    SeparatedValuesSerializer.SerializeToString(
        payload,
        SeparatedValuesDelimiter.Comma
    );

string payloadTSV = 
    SeparatedValuesSerializer.SerializeToString(
        payload, 
        SeparatedValuesDelimiter.Comma
    );
```

## Dependencies

SODA.Utilities has a few dependencies:

  - SODA.NET
  - [Microsoft Access Database Engine 2010 Redistributable](http://www.microsoft.com/en-us/download/details.aspx?id=13255)
(for reading Excel documents through OLEDB)
  - [Microsoft Exchange WebServices 2.1](https://www.nuget.org/packages/EWS-Api-2.1/1.0.0) 
(for talking to an Exchange server)

## Getting Started

SODA.Utilities is available as a [NuGet package](https://www.nuget.org/packages/CSM.SodaDotNet.Utilities/).

    Install-Package CSM.SodaDotNet.Utilities

