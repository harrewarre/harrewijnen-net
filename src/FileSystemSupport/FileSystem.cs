using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Blog.FileSystemSupport;

public interface IFileSystem
{
    string[] GetPostIndex();

    string GetPostContent(string slug);

    string GetPostMetadataContent(string slug);
}

public class FileSystem : IFileSystem
{
    private readonly string _indexSearchPattern = "*.md";
    private readonly string _contentSplitter = "---";
    private readonly string _contentRoot;

    public FileSystem(string contentRoot)
    {
        _contentRoot = $"{contentRoot}/content";
    }

    public string[] GetPostIndex()
    {
        return Directory.GetFiles(_contentRoot, _indexSearchPattern)
                    .Select(p => Path.GetFileNameWithoutExtension(p))
                    .ToArray();
    }

    public string GetPostContent(string slug)
    {
        var path = CreatePostPath(slug);
        return File.ReadAllText(path);
    }

    public string GetPostMetadataContent(string slug)
    {
        var path = CreatePostPath(slug);

        using (var sr = new StreamReader(path))
        {
            var builder = new StringBuilder();

            string line = string.Empty;
            while ((line = sr.ReadLine()) != _contentSplitter)
            {
                builder.Append(line);
            }

            return builder.ToString();
        }
    }

    private string CreatePostPath(string slug)
    {
        return Path.Join(_contentRoot, $"{slug}.md");
    }
}