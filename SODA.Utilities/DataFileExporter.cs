using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using SODA.Models;

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
                exportSV(entities, dataFilePath, ",");
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
                exportSV(entities, dataFilePath, "\t");
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

        // private implementation method for CSV/TSV exporting
        private static void exportSV<T>(IEnumerable<T> entities, string dataFilePath, string delim)
        {
            Type ttype = typeof(T);
            IEnumerable<PropertyInfo> propertiesToExport = ttype.GetProperties();

            bool isDataContract = false;
            List<string> header = new List<string>();

            //is T a class marked with [DataContract]?
            if (ttype.CustomAttributes.Any(c => c.AttributeType == typeof(DataContractAttribute)))
            {
                isDataContract = true;
                //a [DataContract] is supposed to explicitly define which properties are to be exported using the [DataMember] attribute
                propertiesToExport = propertiesToExport.Where(p => p.CustomAttributes.Any(c => c.AttributeType.Equals(typeof(DataMemberAttribute))));
            }
            
            //build the header line
            foreach (var property in propertiesToExport)
            {
                header.Add(property.Name);

                if (isDataContract)
                {
                    //it's possible that a serialization alias, e.g. [DataMember(Name="alias")], has been provided for this property
                    var dataMemberAttribute = property.CustomAttributes.Single(c => c.AttributeType.Equals(typeof(DataMemberAttribute)));
                    if (dataMemberAttribute.NamedArguments.Any(a => a.MemberName.Equals("name", StringComparison.OrdinalIgnoreCase)))
                    {
                        //if so, use the alias instead of the direct property name
                        string dataMemberName = dataMemberAttribute.NamedArguments.Single(a => a.MemberName.Equals("name", StringComparison.OrdinalIgnoreCase)).TypedValue.Value.ToString();
                        header.Remove(property.Name);
                        header.Add(dataMemberName);
                    }
                }
            }

            //write the header line to the data file
            File.WriteAllLines(dataFilePath, new[] { String.Join(delim, header) });

            //now build a line for each entity
            var sb = new StringBuilder();
            var lines = new List<string>();

            foreach (var entity in entities)
            {
                sb.Clear();

                foreach (var property in propertiesToExport)
                {
                    //get the raw value as an object
                    object propertyValue = property.GetValue(entity);
                    //what will eventually be appended to the line for this property
                    string toAppend;

                    //the whitelist contains types that can be written directly as strings
                    if (!jsonSerializeWhiteList.Contains(property.PropertyType))
                    {
                        //this property is not a "simple" type - special consideration should be taken for serialization

                        //locations should be exported in Socrata's desired upload format for *SV: (lat, long)
                        if (property.PropertyType == typeof(LocationColumn))
                        {
                            LocationColumn value = property.GetValue(entity) as LocationColumn;
                            if (String.IsNullOrEmpty(value.Latitude) || String.IsNullOrEmpty(value.Longitude))
                                toAppend = String.Empty;
                            else
                                toAppend = String.Format("({0},{1})", value.Latitude, value.Longitude);
                        }
                        else
                        {
                            toAppend = propertyValue.ToJsonString();
                        }
                    }
                    else
                    {
                        //this property is a "simple" type, get its normalized string representation
                        toAppend = propertyValue.SafeToString().NormalizeQuotes().EscapeDoubleQuotes();
                    }

                    //append this property's value to the line and wrap in quotes for safety
                    sb.AppendFormat(@"""{0}""{1}", toAppend, delim);
                }
                //add this line to the overall collection
                lines.Add(sb.ToString().TrimEnd(delim.ToCharArray()));
            }
            //write the data lines to the data file
            File.AppendAllLines(dataFilePath, lines);
        }

        // a list of primitive types that *will not* be serialized to JSON during CSV/TSV export
        private static Type[] jsonSerializeWhiteList = new[] { 
            typeof(int),
            typeof(int?),
            typeof(long),
            typeof(long?),
            typeof(decimal),
            typeof(decimal?),
            typeof(double),
            typeof(double?),
            typeof(float),
            typeof(float?),
            typeof(DateTime),
            typeof(DateTime?),
            typeof(string),
            typeof(char),
            typeof(Enum),
            typeof(bool),
            typeof(bool?)
        };
    }
}
