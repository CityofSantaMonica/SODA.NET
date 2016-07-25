using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SODA.Models;
using System;

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
            Positions positions = new Positions();
            if (reader.TokenType == JsonToken.StartArray)
            {
                var arrayValues = JArray.Load(reader);

                double arrayIndexOne = (double)arrayValues[0];
                double arrayIndexTwo = (double)arrayValues[1];

                positions.PositionsArray = new double[] { arrayIndexOne, arrayIndexTwo };
            }            
            return positions;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Positions positions = (Positions)value;

            writer.WriteStartArray();
            writer.WriteValue(positions.PositionsArray[0]);
            writer.WriteValue(positions.PositionsArray[1]);
            writer.WriteEndArray();
        }
    }  
}

