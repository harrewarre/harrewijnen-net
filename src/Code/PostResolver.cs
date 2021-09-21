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
        private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string _contentRoot;

        public PostResolver(string webRootPath)
        {
            _contentRoot = Path.Join(webRootPath, "content");
        }

        public IEnumerable<PostMetadata> GetMetadataIndex()
        {
            var posts = Directory.GetFiles(_contentRoot, "*.md");
            var index = posts.Select(p => LoadMetadata(p));

            return index.OrderByDescending(p => p.Created);
        }

        public Post GetPost(string slug)
        {
            try
            {
                var postPath = Path.Join(_contentRoot, $"{slug}.md");
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
                    if (line == "---")
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
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            var postData = File.ReadAllText(postPath);

            var splitter = "---";

            var splitIndex = postData.IndexOf(splitter);
            var mdIndex = splitIndex + 3;

            var frontMatter = postData.Substring(0, splitIndex);
            var markdown = postData.Substring(mdIndex);

            var metadata = JsonSerializer.Deserialize<PostMetadata>(frontMatter);
            metadata.Slug = Path.GetFileNameWithoutExtension(postPath);

            var post = new Post(metadata);
            post.HtmlContent = Markdown.ToHtml(markdown.Trim(), pipeline);

            return post;
        }
    }
}