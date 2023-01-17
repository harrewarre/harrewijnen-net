using Xunit;

using Blog.JsonSupport;
using Blog.Post.Models;
using System;
using System.Text.Json;

namespace test;

public class JsonParserTests
{
    [Fact]
    public void ItShouldReturnMetadata()
    {
        var expected = new BlogPostMetadata {
            Slug = "test-post",
            Title = "Test Post",
            Summary = "Test Post Summary",
            Created = new DateTime(2020,1,1),
            Tags = new string[] { "test" }
        };

        var input = JsonSerializer.Serialize(expected);

        var parser = new JsonParser();
        var result = parser.ParseJson<BlogPostMetadata>(input);

        Assert.Equal(expected.Title, result.Title);
        Assert.Equal(expected.Slug, result.Slug);
        Assert.Equal(expected.Summary, result.Summary);
        Assert.Equal(expected.Created, result.Created);
        Assert.Equal(expected.Tags[0], result.Tags[0]);
    }
}