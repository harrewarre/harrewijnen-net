using Xunit;

using Blog.MarkdownSupport;

namespace test;

public class MarkdownParserTests
{
    [Fact]
    public void ItShouldReturnHtml()
    {
        var input = "test";

        var parser = new MarkdownParser();
        var result = parser.GetHtml(input);

        Assert.Equal("<p>test</p>", result);
    }
}