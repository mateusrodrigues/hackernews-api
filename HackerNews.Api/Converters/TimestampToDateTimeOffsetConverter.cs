using System;
using Newtonsoft.Json;

namespace HackerNews.Api.Converters
{
    public class TimestampToDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return long.TryParse(reader.Value.ToString(), out var timestamp) ? 
                DateTimeOffset.FromUnixTimeSeconds(timestamp) : DateTimeOffset.MinValue;
        }
    }
}