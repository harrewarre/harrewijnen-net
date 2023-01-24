namespace Blog.OpenGraphSupport;

public class OpenGraphData
{
    public OpenGraphData(string title, string description, string url, string type)
    {
        Title = title;
        Description = description;
        Url = url;
        Type = type;
    }

    public string Title { get; }
    public string Description { get; }
    public string Url { get; }
    public string Type { get; }
}