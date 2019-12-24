using System;
using HackerNews.Domain.Converters;
using Newtonsoft.Json;
using Xunit;

namespace HackerNews.UnitTests.Converters
{
    public class TimestampToDateTimeOffsetConverterTests
    {
        private readonly TimestampToDateTimeOffsetConverter _converter;

        public TimestampToDateTimeOffsetConverterTests()
        {
            _converter = new TimestampToDateTimeOffsetConverter();
        }
        
        [Fact]
        public void ReadJson_ValidTimestampInput_ReturnEquivalentDateTimeOffsetObject()
        {
            var conversionObject = new {Time = 1577119099};
            var result = new DateTimeOffset(2019, 12, 23, 16, 38, 19, TimeSpan.Zero);
            var json = JsonConvert.SerializeObject(conversionObject);
            
            var conversion = JsonConvert.DeserializeObject<DestinationFakeObject>(json);
            
            Assert.Equal(result, conversion.Time);
        }

        [Fact]
        public void ReadJson_InvalidTimestampInput_ReturnTheMinimumDateTimeOffsetObject()
        {
            var conversionObject = new {Time = "a"};
            var json = JsonConvert.SerializeObject(conversionObject);

            var conversion = JsonConvert.DeserializeObject<DestinationFakeObject>(json);
            
            Assert.Equal(DateTimeOffset.MinValue, conversion.Time);
        }
    }

    internal class DestinationFakeObject
    {
        [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
        public DateTimeOffset Time { get; set; }
    }
}