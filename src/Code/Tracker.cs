using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace Blog.Code
{
    public interface ITracker
    {
        void TrackPostNotFound(string slug);

        void TrackEmptyBlogAccessed();

        void TrackFeedAccessed();

        void TrackPostFound(string slug);
    }

    public class Tracker : ITracker
    {
        private readonly TelemetryClient _telemetry;

        public Tracker(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        public void TrackEmptyBlogAccessed()
        {
            _telemetry.TrackEvent("EMPTY_BLOG_ACCESSED");
        }

        public void TrackFeedAccessed()
        {
            _telemetry.TrackEvent("RSS_FEED_ACCESSED");
        }

        public void TrackPostNotFound(string slug)
        {
            var props = new Dictionary<string,string>();
            props.Add("slug", slug);

            _telemetry.TrackEvent("POST_NOT_FOUND", props);
        }

        public void TrackPostFound(string slug)
        {
            var props = new Dictionary<string,string>();
            props.Add("slug", slug);

            _telemetry.TrackEvent("POST_FOUND", props);
        }
    }
}