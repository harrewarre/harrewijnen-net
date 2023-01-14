namespace Blog.Post.Models;

public class BlogPost
{
    public BlogPost(BlogPostMetadata metadata)
    {
        Metadata = metadata;
    }

    public BlogPostMetadata Metadata { get; }
    public string HtmlContent { get; set; }
}