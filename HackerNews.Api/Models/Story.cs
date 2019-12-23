using System;
using HackerNews.Api.Converters;
using Newtonsoft.Json;

namespace HackerNews.Api.Models
{
    public class Story
    {
        public string Title { get; set; }
        
        [JsonProperty("url")]
        public Uri Uri { get; set; }
        
        [JsonProperty("by")]
        public string PostedBy { get; set; }
        
        [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
        public DateTimeOffset Time { get; set; }
        
        public int Score { get; set; }
        
        [JsonProperty("descendants")]
        public int CommentCount { get; set; }
    }
}