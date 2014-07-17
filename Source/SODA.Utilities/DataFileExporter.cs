using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SODA.Utilities
{
    public class DataFileExporter
    {
        public static void ExportTSV<T>(IEnumerable<T> entities)
        {
            exportSV(entities);
        }

        public static void ExportCSV<T>(IEnumerable<T> entities)
        {
            exportSV(entities, ",");
        }

        public static void ExportJSON<T>(IEnumerable<T> entities)
        {
            string json = JsonConvert.SerializeObject(entities);

            File.WriteAllText("data.json", json);
        }

        private static void exportSV<T>(IEnumerable<T> entities, string delim = "\t")
        {
            string dataFile = "data.tsv";

            if (delim == ",")
            {
                dataFile = "data.csv";
            }
            else
            {
                delim = "\t";
            }

            var allProperties = typeof(T).GetProperties();

            File.WriteAllLines(dataFile, new[] { String.Join(delim, allProperties.Select(p => p.Name)) });

            var sb = new StringBuilder();
            var records = new List<string>();

            foreach (var entity in entities)
            {
                sb.Clear();

                foreach (var property in allProperties)
                {
                    object value = property.GetValue(entity);
                    string toAppend = String.Format(@"""{0}""{1}", value, delim);

                    if (!(value == null || jsonSerializeWhiteList.Contains(property.PropertyType)))
                    {
                        string json = JsonConvert.SerializeObject(value);
                        toAppend = String.Format(@"""{0}""{1}", json, delim);
                    }

                    sb.Append(toAppend);
                }

                records.Add(sb.ToString().TrimEnd(delim.ToCharArray()));
            }

            File.AppendAllLines(dataFile, records);
        }

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
            typeof(string)
        };
    }
}
