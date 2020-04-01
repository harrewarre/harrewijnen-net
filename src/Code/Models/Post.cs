namespace Blog.Code.Models
{
    public class Post
    {
        public Post(PostMetadata metadata)
        {
            Metadata = metadata;
        }

        public PostMetadata Metadata { get; }
        public string HtmlContent { get; set; }
    }
}