using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SODA.Models;
using System;
using System.Linq;

namespace SODA.Utilities
{
    class PositionsJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Positions).IsAssignableFrom(objectType);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var arrayValues = JArray.Load(reader).Values<double>().ToArray();

            return new Positions(arrayValues);           
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var positions = (Positions)value;
            
            writer.WriteStartArray();
            foreach (var item in positions.PositionsArray)
            {
                writer.WriteValue(item);
            }

            writer.WriteEndArray();
        }
    }
}

