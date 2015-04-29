using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using SODA.Models;

namespace SODA.Utilities
{
    /// <summary>
    /// A helper class for serializing data to a "separated values" representation.
    /// </summary>
    public class SeparatedValuesSerializer
    {
        /// <summary>
        /// Gets the corresponding delimiter string for the specified <see cref="SeparatedValuesDelimiter"/>
        /// </summary>
        public static string DelimiterString(SeparatedValuesDelimiter delimiter)
        {
            string delimiterString = String.Empty;

            if (delimiter == SeparatedValuesDelimiter.Comma)
                delimiterString = ",";
            else if (delimiter == SeparatedValuesDelimiter.Tab)
                delimiterString = "\t";

            return delimiterString;
        }

        /// <summary>
        /// Serialize the specified entity collection to a string using the specified delimiter character, optionally generating a header row
        /// </summary>
        /// <typeparam name="T">The type of entities in the collection.</typeparam>
        /// <param name="entities">The collection to serialize.</param>
        /// <param name="delimiter">A <see cref="SeparatedValuesDelimiter"/> indicating how to separate individual fields in the output string.</param>
        /// <param name="generateHeader">True to generate a header row for the serialized fields, false to skip the header row. The default is true.</param>
        /// <returns>A string reperesentation of the entity collection.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid <paramref name="delimiter"/> is provided.</exception>
        public static string SerializeToString<T>(IEnumerable<T> entities, SeparatedValuesDelimiter delimiter, bool generateHeader = true)
        {
            string delimiterString = DelimiterString(delimiter);
            if (String.IsNullOrEmpty(delimiterString))
                throw new ArgumentOutOfRangeException("delimiter");

            bool isDataContract = false;
            var propertiesToExport = getExportProperties<T>(out isDataContract);

            StringBuilder serializedData = new StringBuilder();

            if (generateHeader)
            {
                string line = generateHeaderLine(propertiesToExport, isDataContract, delimiterString);
                serializedData.AppendLine(line);
            }

            foreach (var entity in entities)
            {
                var line = generateEntityLine(entity, propertiesToExport, delimiterString);
                serializedData.AppendLine(line);
            }

            return serializedData.ToString().Trim();
        }

        private static IEnumerable<PropertyInfo> getExportProperties<T>(out bool isDataContract)
        {
            Type ttype = typeof(T);
            IEnumerable<PropertyInfo> propertiesToExport = ttype.GetProperties();
            isDataContract = false;

            if (ttype.CustomAttributes.Any(c => c.AttributeType == typeof(DataContractAttribute)))
            {
                isDataContract = true;
                //a [DataContract] is supposed to explicitly define which properties are to be exported using the [DataMember] attribute
                propertiesToExport = propertiesToExport.Where(p => p.CustomAttributes.Any(c => c.AttributeType.Equals(typeof(DataMemberAttribute))));
            }

            return propertiesToExport;
        }

        private static string generateHeaderLine(IEnumerable<PropertyInfo> propertiesToExport, bool isDataContract, string delimiter)
        {
            var header = new List<string>();

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

            return String.Join(delimiter, header);
        }

        private static string generateEntityLine<T>(T entity, IEnumerable<PropertyInfo> propertiesToExport, string delimiter)
        {
            StringBuilder lineBuilder = new StringBuilder();

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
                lineBuilder.AppendFormat(@"""{0}""{1}", toAppend, delimiter);
            }

            return lineBuilder.ToString().TrimEnd(delimiter.ToCharArray());
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
