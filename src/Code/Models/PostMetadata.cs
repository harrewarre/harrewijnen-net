using System;
using System.Text.Json.Serialization;

namespace Blog.Code.Models
{
    public class PostMetadata
    {
        [JsonPropertyName("slug")]
        public string Slug { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        public string Location
        {
            get
            {
                return $"/blog/{Slug}";
            }
        }
    }
}