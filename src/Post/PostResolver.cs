using System.Collections.Generic;
using System.Linq;
using Blog.Post.Models;
using System;
using Blog.MarkdownSupport;
using Blog.JsonSupport;

namespace Blog.Post;

public interface IPostResolver
{
    IEnumerable<BlogPostMetadata> GetMetadataIndex();
    BlogPost GetPost(string slug);
}

public class PostResolver : IPostResolver
{
    private readonly string _contentSplitter = "---";

    private readonly IPostRepository _postRepository;
    private readonly IMarkdownParser _markdownParser;
    private readonly IJsonParser _jsonParser;

    public PostResolver(IPostRepository repo, IMarkdownParser markdownParser, IJsonParser jsonParser)
    {
        _postRepository = repo;
        _markdownParser = markdownParser;
        _jsonParser = jsonParser;
    }

    public IEnumerable<BlogPostMetadata> GetMetadataIndex()
    {
        var slugs = _postRepository.GetPostIndex();
        var index = slugs.Select(slug => LoadMetadata(slug));

        return index.OrderByDescending(p => p.Created);
    }

    public BlogPost GetPost(string slug)
    {
        try
        {
            var postData = _postRepository.GetPostContent(slug);

            var splitIndex = postData.IndexOf(_contentSplitter);
            var contentSplitIndex = splitIndex + _contentSplitter.Length;

            var frontMatter = postData.Substring(0, splitIndex);
            var content = postData.Substring(contentSplitIndex);

            var metadata = _jsonParser.ParseJson<BlogPostMetadata>(frontMatter);
            metadata.Slug = slug;

            var post = new BlogPost(metadata);
            post.HtmlContent = _markdownParser.GetHtml(content);

            Console.WriteLine($"--- Loaded data for page: {slug}");

            return post;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"--- Failed to load post: {slug} !! {ex.Message} | {ex.StackTrace}");
            return null;
        }
    }

    private BlogPostMetadata LoadMetadata(string slug)
    {
        Console.WriteLine($"--- Loading metadata for {slug}");

        var metadataContent = _postRepository.GetPostMetadataContent(slug);

        var metadata = _jsonParser.ParseJson<BlogPostMetadata>(metadataContent);
        metadata.Slug = slug;

        return metadata;
    }
}