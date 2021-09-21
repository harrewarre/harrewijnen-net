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
        private readonly string _postExtentionSearchPattern = "*.md";
        private readonly string _postExtenion = ".md";
        private readonly string _contentDirectoryName = "content";
        private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string _contentRoot;

        public PostResolver(string webRootPath)
        {
            _contentRoot = Path.Join(webRootPath, _contentDirectoryName);

            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
        }

        public IEnumerable<PostMetadata> GetMetadataIndex()
        {
            var posts = Directory.GetFiles(_contentRoot, _postExtentionSearchPattern);
            var index = posts.Select(p => LoadMetadata(p));

            return index.OrderByDescending(p => p.Created);
        }

        public Post GetPost(string slug)
        {
            try
            {
                var postPath = Path.Join(_contentRoot, $"{slug}{_postExtenion}");
                return LoadPost(postPath);
            }
            catch
            {
                return null;
            }
        }

        private PostMetadata LoadMetadata(string postPath)
        {
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

                Console.WriteLine(content);

                var metadata = JsonSerializer.Deserialize<PostMetadata>(content);
                metadata.Slug = Path.GetFileNameWithoutExtension(postPath);

                return metadata;
            }
        }

        private Post LoadPost(string postPath)
        {
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

            return post;
        }
    }
}