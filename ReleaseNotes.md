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
