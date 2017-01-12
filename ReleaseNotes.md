### New in 0.7.0 (Released 2017/01/12)

*It is highly recommended to update to this version, as it features improved security following Socrata's deprecation of TLS 1.0*

Security enhancements

  - Disable security protocols lower than TLS 1.1 [#65](https://github.com/CityofSantaMonica/SODA.NET/issues/65)

Bug fixes

  - `SoqlQuery` no longer inserts default query arguments [#59](https://github.com/CityofSantaMonica/SODA.NET/issues/59)

New features

  - `SoqlQuery` supports the `$having` clause [#60](https://github.com/CityofSantaMonica/SODA.NET/issues/60)
  - `SoqlQuery` supports the `$query` clause [#63](https://github.com/CityofSantaMonica/SODA.NET/issues/63)

### New in 0.6.0 (Released 2016/06/02)

Bug fixes

  - `SodaClient` no longer requires an AppToken [#49](https://github.com/CityofSantaMonica/SODA.NET/issues/49)
  - `Query` methods behave as advertised and return full result sets [#56](https://github.com/CityofSantaMonica/SODA.NET/issues/56)

Deprecation Notices

  - `ExcelOleDbHelper` in `SODA.Utilities` was deprecated in `v0.5.0` and has now been removed.

New features

  - `SodaClient` can issue queries directly [#54](https://github.com/CityofSantaMonica/SODA.NET/issues/54)

### New in 0.5.0 (Released 2015/08/12)

Dependency Updates

  - `Newtonsoft.Json` upgraded to 7.0.1 [#43](https://github.com/CityofSantaMonica/SODA.NET/issues/43)

Deprecation Notices

  - `ExcelOleDbHelper` in `SODA.Utilities` has been deprecated and replaced by `ExcelDataReaderHelper`. `ExcelOleDbHelper` will be removed in `v0.6.0`.

New features

  - `ExcelDataReaderHelper` for reading data from Excel documents [#46](https://github.com/CityofSantaMonica/SODA.NET/pull/46) via @allejo
  - `Ews2010Sp2Client` for utilizing EWS against Exchange 2010 SP2

### New in 0.4.1 (Released 2015/06/11)

Bug fixes

  - `SoqlQuery.MaximumLimit` increased to 50K [#39](https://github.com/CityofSantaMonica/SODA.NET/issues/39) via @chrismetcalf

### New in 0.4.0 (Released 2015/05/07)

Dependency Updates

  - `Newtonsoft.Json` upgraded to 6.0.8

New features

  - Overload to skip header for `SeparatedValuesSerializer` [#31](https://github.com/CityofSantaMonica/SODA.NET/issues/31)
  - Optional `RequestTimeout` property on `SodaClient` [#28](https://github.com/CityofSantaMonica/SODA.NET/issues/28)

### New in 0.3.0 (Released 2015/01/08)

Dependency Updates

  - `Newtonsoft.Json` upgraded to 6.0.7
  - `NUnit` (for test projects) upgraded to 2.6.4
  
New features

  - Implementation of SODA [`PhoneColumn`](https://support.socrata.com/hc/en-us/articles/202949918-Importing-Data-Types-and-You-) [#24](https://github.com/CityofSantaMonica/SODA.NET/pull/24) via @mickmorbitzer
  - Added a Uri helper for Foundry-style API documentation pages [#22](https://github.com/CityofSantaMonica/SODA.NET/issues/22)

### New in 0.2.1 (Released 2014/12/02)

Bug fixes
  
  - Soql Limit and Offset throw exceptions for out of range values [#18](https://github.com/CityofSantaMonica/SODA.NET/issues/18)
  - Fixed column aliasing bug using Soql As [#17](https://github.com/CityofSantaMonica/SODA.NET/issues/17) 

### New in 0.2.0 (Released 2014/11/03)

Dependency Updates

  - `Newtonsoft.Json` upgraded to 6.0.6 [#15](https://github.com/CityofSantaMonica/SODA.NET/issues/15)

New features

  - Convenience `Query` method on `Resource<TRow>` for results that are collections of `TRow` [#12](https://github.com/CityofSantaMonica/SODA.NET/issues/12)
  - `SeparatedValuesSerializer` in `SODA.Utilities` can serialize entities to CSV/TSV strings in memory [#2](https://github.com/CityofSantaMonica/SODA.NET/issues/2)

### New in 0.1.2 (Released 2014/10/01)

Minor bug fixes and cleanup

  - `SodaResult` members aren't publicly settable [#9](https://github.com/CityofSantaMonica/SODA.NET/issues/9)
  - `unwrapExceptionMessage` moved to a public extension of `WebException` [#8](https://github.com/CityofSantaMonica/SODA.NET/issues/8)
  - `Newtonsoft.Json` upgraded to 6.0.5 [#6](https://github.com/CityofSantaMonica/SODA.NET/issues/6)
  - `SodaResult` correctly deserializes the `By RowIdentifier` value [#5](https://github.com/CityofSantaMonica/SODA.NET/issues/5)
  - Some common assembly information [moved to a shared solution-level file](https://github.com/CityofSantaMonica/SODA.NET/commit/5cf686018b49fcd7883561b8a37ec214246d07e6). This will help with deployment

### New in 0.1.1 (Released 2014/09/16)

Minor bug fixes and cleanup
  
  - Replace with CSV correctly parses `SodaResult` [#4](https://github.com/CityofSantaMonica/SODA.NET/issues/4) 
  - Upsert with CSV correctly parses `SodaResult` [#3](https://github.com/CityofSantaMonica/SODA.NET/issues/3)

### New in 0.1.0 (Released 2014/08/28)

  - Initial release!
  - This library is under active development. As such, pre-v1.0 versions may introduce breaking changes until things stabilize around v1.0. Every effort will be made to ensure that any breaking change is well documented and that appropriate workarounds are suggested. 
