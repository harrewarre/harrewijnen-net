using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace Blog.Code
{
    public interface IEventLogger
    {
        void LogPageview(string slug);

        void LogPageMiss(string path);
    }

    public class EventLogger : IEventLogger
    {
        private readonly TelemetryClient _client;

        public EventLogger(TelemetryClient telemetryClient)
        {
            _client = telemetryClient;
        }

        public void LogPageMiss(string path)
        {
            var props =  new Dictionary<string,string>();
            props.Add("path", path);

            _client.TrackEvent("PAGE_NOT_FOUND", props);
        }

        public void LogPageview(string slug)
        {
            var props =  new Dictionary<string,string>();
            props.Add("slug", slug);

            _client.TrackPageView(slug);
            _client.TrackEvent("PAGE_VIEWED", props);
        }
    }
}