using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Utilities.Net45.Tests")]
[assembly: InternalsVisibleTo("Utilities.NetCore22.Tests")]

namespace SODA.Utilities
{
    /// <summary>
    /// A helper class for exporting a collection of objects to various text-based file formats.
    /// </summary>
    public class DataFileExporter
    {
        /// <summary>
        /// The default filepath used for exporting CSV data.
        /// </summary>
        public static readonly string DefaultCSVPath = "data.csv";

        /// <summary>
        /// The default filepath used for exporting TSV data.
        /// </summary>
        public static readonly string DefaultTSVPath = "data.tsv";

        /// <summary>
        /// The default filepath used for exporting JSON data.
        /// </summary>
        public static readonly string DefaultJSONPath = "data.json";

        /// <summary>
        /// Export a collection of objects to DataFileExporter.DefaultCSVPath (in the working directory).
        /// </summary>
        /// <param name="entities">A collection of objects to export as Comma Separated Values.</param>
        /// <remarks>
        /// String values containing double-quotes will have the double-quotes escaped.
        /// 
        /// Complex types will be serialized to JSON. In other words, if you export a collection as CSV/TSV,
        /// and items in that collection contain a property with a complex type (i.e. not a System primitive)
        /// that property will be serialized to its JSON string representation.
        /// 
        /// DataFileExporter makes every attempt to honor DataContracts.
        /// </remarks>
        public static void ExportCSV<T>(IEnumerable<T> entities)
        {
            ExportCSV(entities, "data.csv");
        }

        /// <summary>
        /// Export a collection of objects to the specified file in Comma Separated Values format, overwriting any existing data.
        /// </summary>
        /// <param name="entities">A collection of objects to export as Comma Separated Values.</param>
        /// <param name="dataFilePath">The path to a .csv file in a writeable directory.</param>
        /// <remarks>
        /// String values containing double-quotes will have the double-quotes escaped.
        /// 
        /// Complex types will be serialized to JSON. In other words, if you export a collection as CSV/TSV,
        /// and items in that collection contain a property with a complex type (i.e. not a System primitive)
        /// that property will be serialized to its JSON string representation.
        /// 
        /// DataFileExporter makes every attempt to honor DataContracts.
        /// </remarks>
        public static void ExportCSV<T>(IEnumerable<T> entities, string dataFilePath)
        {
            if (!String.IsNullOrEmpty(dataFilePath) && dataFilePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                string serializedData = SeparatedValuesSerializer.SerializeToString(entities, SeparatedValuesDelimiter.Comma);
                File.WriteAllText(dataFilePath, serializedData);
                return;
            }

            throw new ArgumentException("Not a valid CSV file path", "dataFilePath");
        }
        
        /// <summary>
        /// Export a collection of objects to DataFileExporter.DefaultTSVPath (in the working directory).
        /// </summary>
        /// <param name="entities">A collection of objects to export as Tab Separated Values.</param>
        /// <remarks>
        /// String values containing double-quotes will have the double-quotes escaped.
        /// 
        /// Complex types will be serialized to JSON. In other words, if you export a collection as CSV/TSV,
        /// and items in that collection contain a property with a complex type (i.e. not a System primitive)
        /// that property will be serialized to its JSON string representation.
        /// 
        /// DataFileExporter makes every attempt to honor DataContracts.
        /// </remarks>
        public static void ExportTSV<T>(IEnumerable<T> entities)
        {
            ExportTSV(entities, "data.tsv");
        }

        /// <summary>
        /// Export a collection of objects to the specified file in Tab Separated Values format, overwriting any existing data.
        /// </summary>
        /// <param name="entities">A collection of objects to export as Tab Separated Values.</param>
        /// <param name="dataFilePath">The path to a .tsv file in a writeable directory.</param>
        /// <remarks>
        /// String values containing double-quotes will have the double-quotes escaped.
        /// 
        /// Complex types will be serialized to JSON. In other words, if you export a collection as CSV/TSV,
        /// and items in that collection contain a property with a complex type (i.e. not a System primitive)
        /// that property will be serialized to its JSON string representation.
        /// 
        /// DataFileExporter makes every attempt to honor DataContracts.
        /// </remarks>
        public static void ExportTSV<T>(IEnumerable<T> entities, string dataFilePath)
        {
            if (!String.IsNullOrEmpty(dataFilePath) && dataFilePath.EndsWith(".tsv", StringComparison.OrdinalIgnoreCase))
            {
                string serializedData = SeparatedValuesSerializer.SerializeToString(entities, SeparatedValuesDelimiter.Tab);
                File.WriteAllText(dataFilePath, serializedData);
                return;
            }

            throw new ArgumentException("Not a valid TSV file path", "dataFilePath");
        }
        
        /// <summary>
        /// Export a collection of objects to DataFileExporter.DefaultJSONPath (in the working directory).
        /// </summary>
        /// <param name="entities">A collection of objects to export as JSON.</param>
        public static void ExportJSON<T>(IEnumerable<T> entities)
        {
            ExportJSON(entities, "data.json");
        }

        /// <summary>
        /// Export a collection of objects to the specified file, overwriting any existing data.
        /// </summary>
        /// <param name="dataFilePath">The path to a .json file in a writeable directory.</param>
        /// <param name="entities">A collection of objects to export as JSON.</param>
        public static void ExportJSON<T>(IEnumerable<T> entities, string dataFilePath)
        {
            if (!String.IsNullOrEmpty(dataFilePath) && dataFilePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                string json = entities.ToJsonString();
                File.WriteAllText(dataFilePath, json);
                return;
            }

            throw new ArgumentException("Not a valid JSON file path", "dataFilePath");
        }
    }
}
