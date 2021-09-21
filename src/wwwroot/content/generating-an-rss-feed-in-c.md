{
  "slug": "generating-an-rss-feed-in-c",
  "title": "Generating an RSS feed in C#",
  "summary": "A hobby project of mine needed to generate an RSS feed.",
  "tags": [
    "C#",
    "RSS",
    "ASP.NET",
    "System.ServiceModel",
    "itq"
  ],
  "created": "2015-04-26T00:00:00"
}
---
# Generating an RSS feed in C#

A hobby project of mine needed to generate an RSS feed. I was already doing this by just generating and XML file in the webroot and linking to that on the frontpage. But during this coding session, where all the data is pulled straight from the markdown files I wanted to create a controller action to do the generating for me.

First things first, there is an assembly available out of the box in the .NET Framework. All you need is to make reference to **System.ServiceModel.Syndication** and you are ready to start writing code. The classes we are using are

- SyndicationItem
- SyndicationFeed
- TextSyndicationContent

We want to write the result straigt into the response, so a FileResult will serve as a baseclass for our RssResult class. First you'll want to create a new constructor that takes in the items that you want to display in the feed. Don't forget to call the base constructor with the right content type (**application/rss+xml** in our case) to tell the requesting client that we are serving an RSS feed.

By overriding the WriteFile method, we take control of writing to the response stream. To make my code independent of site location I first resolve the domain from the request URL. Next we convert the items we want in the feed to SyndicationItems. A quick LINQ select statement makes short work of the conversion.

To create a feed, we need to put the SyndicationItems in a SyndicationFeed. Set the title and description of the feed and you are ready to write to the response stream. Create a new XmlWriter object and point it into the outputstream of the response we are going to send to the client. All that is left then is to put the writer to work using the WriteTo method on the SyndicationFeed's Rss20Formatter.

Pretty easy right? :-) Copy-Pasta ready code below! Enjoy.

    public class RssResult : FileResult
    {
        private readonly IEnumerable<BlogPost> FeedItems;
        private readonly Uri Url;
    
        public RssResult(IEnumerable<BlogPost> items)
            : base("application/rss+xml")
        {
            this.FeedItems = items;
            this.Url = HttpContext.Current.Request.Url;
        }
    
        protected override void WriteFile(HttpResponseBase response)
        {
            var sourceUrl = this.Url.GetLeftPart(UriPartial.Authority);
    
            var feedItems = this.FeedItems.Select(p => new SyndicationItem(p.Title, p.Body, new Uri(string.Format("{0}/post/{1}", sourceUrl, p.Slug)))
            {
                PublishDate = new DateTimeOffset(p.CreateDate)
            });
    
            var feed = new SyndicationFeed(feedItems)
            {
                Title = new TextSyndicationContent("Harrewijnen.net", TextSyndicationContentKind.Html),
                Description = new TextSyndicationContent("From the mind of a coder/gamer")
            };
    
            using (XmlWriter writer = XmlWriter.Create(response.OutputStream))
            {
               feed.GetRss20Formatter().WriteTo(writer);
            }
        }
    }