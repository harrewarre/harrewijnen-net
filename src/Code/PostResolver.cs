using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Markdig;
using Blog.Code.Models;

namespace Blog.Code
{
    public interface IPostResolver
    {
        Task<IEnumerable<PostMetadata>> GetMetadataIndex();
        Task<Post> GetPost(string slug);
    }

    public class PostResolver : IPostResolver
    {
        private readonly string _metadataFileName = "meta.json";
        private readonly string _contentFilename = "post.md";

        private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly string _contentRoot;

        public PostResolver(string webRootPath)
        {
            _contentRoot = Path.Join(webRootPath, "content");
        }

        public async Task<IEnumerable<PostMetadata>> GetMetadataIndex()
        {
            var paths = Directory.GetDirectories(_contentRoot);
            var index = await Task.WhenAll(paths.Select(p => LoadMetadata(p)));

            return index.OrderByDescending(p => p.Created);
        }

        public async Task<Post> GetPost(string slug)
        {
            try
            {
                var contentPath = Path.Join(_contentRoot, slug);

                var meta = await LoadMetadata(contentPath);
                var content = await LoadContent(contentPath);

                var post = new Post(meta);
                post.HtmlContent = content;

                return post;
            }
            catch
            {
                return null;
            }
        }

        private async Task<PostMetadata> LoadMetadata(string path)
        {
            var metaPath = Path.Join(path, _metadataFileName);
            using (FileStream fs = File.OpenRead(metaPath))
            {
                var metadata = await JsonSerializer.DeserializeAsync<PostMetadata>(fs, _metaSerializeOptions);
                metadata.Slug = new DirectoryInfo(path).Name;

                return metadata;
            }
        }

        private async Task<string> LoadContent(string path)
        {
            var contentPath = Path.Join(path, _contentFilename);

            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            var markdown = await File.ReadAllTextAsync(contentPath);
            return Markdown.ToHtml(markdown, pipeline);
        }
    }
}