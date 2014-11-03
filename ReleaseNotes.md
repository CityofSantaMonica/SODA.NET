### New in 0.2.0 (Released 2014/11/03)

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
