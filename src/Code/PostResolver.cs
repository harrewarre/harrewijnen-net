using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Markdig;
using Blog.Code.Models;
using System.Text;
using System;
using Blog.FileSystemSupport;

namespace Blog.Code
{
    public interface IPostResolver
    {
        IEnumerable<PostMetadata> GetMetadataIndex();
        Post GetPost(string slug);
    }

    public class PostResolver : IPostResolver
    {
        private readonly string _contentSplitter = "---";

        private readonly MarkdownPipeline _markdownPipeline;
        private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IFileSystem _fileSystem;

        public PostResolver(IFileSystem fileSystem)
        {
            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            _fileSystem = fileSystem;
        }

        public IEnumerable<PostMetadata> GetMetadataIndex()
        {
            var slugs = _fileSystem.GetPostIndex();
            var index = slugs.Select(slug => LoadMetadata(slug));

            return index.OrderByDescending(p => p.Created);
        }

        public Post GetPost(string slug)
        {
            try
            {
                var postData = _fileSystem.GetPostContent(slug);

                var splitIndex = postData.IndexOf(_contentSplitter);
                var contentSplitIndex = splitIndex + _contentSplitter.Length;

                var frontMatter = postData.Substring(0, splitIndex);
                var content = postData.Substring(contentSplitIndex);

                var metadata = ParseMetadata(frontMatter);
                metadata.Slug = slug;

                var post = new Post(metadata);
                post.HtmlContent = ParseContent(content);

                Console.WriteLine($"--- Loaded data for page: {slug}");

                return post;
            } 
            catch (Exception ex)
            {
                Console.Error.WriteLine($"--- Failed to load post: {slug} !! {ex.Message} | {ex.StackTrace}");
                return null;
            }
        }

        private PostMetadata LoadMetadata(string slug)
        {
            Console.WriteLine($"--- Loading metadata for {slug}");
            
            var metadataContent = _fileSystem.GetPostMetadataContent(slug);

            var metadata = ParseMetadata(metadataContent);
            metadata.Slug = slug;

            return metadata;
        }

        private PostMetadata ParseMetadata(string metadataJson)
        {
            return JsonSerializer.Deserialize<PostMetadata>(metadataJson);
        }

        private string ParseContent(string content)
        {
            return Markdown.ToHtml(content.Trim(), _markdownPipeline);
        }
    }
}