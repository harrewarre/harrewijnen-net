using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Markdig;
using Blog.Code.Models;
using System.Text;
using System;

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
        private readonly string _postSearchPattern = "*.md";
        private readonly string _postExtenion = ".md";
        private readonly string _contentDirectoryName = "content";
        private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string _contentRoot;
        private readonly IEventLogger _eventLogger;

        public PostResolver(string webRootPath, IEventLogger eventLogger)
        {
            _contentRoot = Path.Join(webRootPath, _contentDirectoryName);

            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            
            _eventLogger = eventLogger;
        }

        public IEnumerable<PostMetadata> GetMetadataIndex()
        {
            var posts = Directory.GetFiles(_contentRoot, _postSearchPattern);
            var index = posts.Select(p => LoadMetadata(p));

            return index.OrderByDescending(p => p.Created);
        }

        public Post GetPost(string slug)
        {
            try
            {
                var postPath = Path.Join(_contentRoot, $"{slug}{_postExtenion}");
                var postData = File.ReadAllText(postPath);

                var splitter = _contentSplitter;

                var splitIndex = postData.IndexOf(splitter);
                var mdIndex = splitIndex + _contentSplitter.Length;

                var frontMatter = postData.Substring(0, splitIndex);
                var markdown = postData.Substring(mdIndex);

                var metadata = JsonSerializer.Deserialize<PostMetadata>(frontMatter);
                metadata.Slug = Path.GetFileNameWithoutExtension(postPath);

                var post = new Post(metadata);
                post.HtmlContent = Markdown.ToHtml(markdown.Trim(), _markdownPipeline);

                _eventLogger.LogPageview(slug);
                Console.WriteLine($"Loaded data for page: {slug}");

                return post;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load post: {slug} !! {ex.Message} | {ex.StackTrace}");
                _eventLogger.LogPageMiss(slug);
                return null;
            }
        }

        private PostMetadata LoadMetadata(string postPath)
        {
            Console.WriteLine($"--- Loading metadata for {postPath}");
            
            using (var sr = new StreamReader(postPath))
            {
                var builder = new StringBuilder();

                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == _contentSplitter)
                    {
                        break;
                    }

                    builder.Append(line);
                }

                var content = builder.ToString();

                var metadata = JsonSerializer.Deserialize<PostMetadata>(content);
                metadata.Slug = Path.GetFileNameWithoutExtension(postPath);

                return metadata;
            }
        }
    }
}