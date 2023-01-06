using System.Collections.Generic;
using System.Linq;
using Blog.Code.Models;
using System;
using Blog.Repository;
using Blog.MarkdownSupport;
using Blog.JsonSupport;

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

        private readonly IPostRepository _postRepository;
        private readonly IMarkdownParser _markdownParser;
        private readonly IJsonParser _jsonParser;

        public PostResolver(IPostRepository repo, IMarkdownParser markdownParser, IJsonParser jsonParser)
        {
            _postRepository = repo;
            _markdownParser = markdownParser;
            _jsonParser = jsonParser;
        }

        public IEnumerable<PostMetadata> GetMetadataIndex()
        {
            var slugs = _postRepository.GetPostIndex();
            var index = slugs.Select(slug => LoadMetadata(slug));

            return index.OrderByDescending(p => p.Created);
        }

        public Post GetPost(string slug)
        {
            try
            {
                var postData = _postRepository.GetPostContent(slug);

                var splitIndex = postData.IndexOf(_contentSplitter);
                var contentSplitIndex = splitIndex + _contentSplitter.Length;

                var frontMatter = postData.Substring(0, splitIndex);
                var content = postData.Substring(contentSplitIndex);

                var metadata = _jsonParser.ParseJson<PostMetadata>(frontMatter);
                metadata.Slug = slug;

                var post = new Post(metadata);
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

        private PostMetadata LoadMetadata(string slug)
        {
            Console.WriteLine($"--- Loading metadata for {slug}");
            
            var metadataContent = _postRepository.GetPostMetadataContent(slug);

            var metadata = _jsonParser.ParseJson<PostMetadata>(metadataContent);
            metadata.Slug = slug;

            return metadata;
        }
    }
}