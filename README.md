# SODA.NET

SODA is a client library targeting .NET 4.5 and above that provides an easy way to interact with the
[Socrata Open Data API](http://dev.socrata.com) (SODA).

## Usage examples

Simple, read-only access

```c#
var client = new SodaClient("data.smgov.net", "AppToken");

//read metadata of a dataset
var metadata = client.GetMetadata("1234-wxyz");
Console.WriteLine("Dataset '{0}' has {1} views.", metadata.Name, metadata.ViewsCount);

//get a reference to the resource itself
//Resource is a generic type, the type parameter represents the underlying rows of the resource
var dataset = client.GetResource<Dictionary<string, object>>("1234-wxyz");

//of course, a custom type can be used as long as the type is JSON serializable.
var dataset = client.GetResource<MyClass>("1234-wxyz");

//Resource objects read their own data
var allRows = dataset.GetRows();
var first10Rows = dataset.GetRows(10);

//collections of an arbitrary type can be returned using SoQL and a fluent query building syntax
var soql = new SoqlQuery().Select("column1", "column2").Where("something > nothing").Group("column3");
var results = dataset.Query<MyOtherClass>(soql);
```

`SodaClient` is also capable of performing write operations

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

## Copyright and License

Copyright 2014 City of Santa Monica, CA

Licensed under the [MIT License](https://github.com/CityOfSantaMonica/SODA.net/blob/master/LICENSE.txt)