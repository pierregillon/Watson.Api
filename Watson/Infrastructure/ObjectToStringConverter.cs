using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Watson.Domain
{
    public class ObjectToStringConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var constructor = objectType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { typeof(string) }, null);
                return constructor.Invoke(new [] { (string)reader.Value });
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
                writer.Flush();
            }
        }
}