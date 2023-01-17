using Markdig;

namespace Blog.MarkdownSupport;

public interface IMarkdownParser
{
    string GetHtml(string markdown);
}

public class MarkdownParser : IMarkdownParser
{
    private readonly MarkdownPipeline _markdownPipeline;

    public MarkdownParser()
    {
        _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
    }

    public string GetHtml(string markdown)
    {
        return Markdown.ToHtml(markdown.Trim(), _markdownPipeline).Trim();
    }
}