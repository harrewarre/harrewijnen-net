using System;
using System.Text.Json.Serialization;

namespace Blog.Post.Models;

public class BlogPostMetadata
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
