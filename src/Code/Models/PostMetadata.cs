using System;

namespace Blog.Code.Models
{
    public class PostMetadata
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public DateTime Created { get; set; }
        public string Location
        {
            get
            {
                return $"/blog/{Slug}";
            }
        }
    }
}