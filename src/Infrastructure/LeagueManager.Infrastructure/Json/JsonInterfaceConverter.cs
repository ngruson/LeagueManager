using Newtonsoft.Json;
using System;

namespace LeagueManager.Infrastructure.Json
{
    public class JsonInterfaceConverter<TImpl, TIntf> : JsonConverter where TImpl : TIntf
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(TIntf);

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
            => serializer.Deserialize<TImpl>(reader);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) 
            => serializer.Serialize(writer, value);
        
    }
}