using Microsoft.Toolkit.Parsers.Rss;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WykoBot
{
    public class FeedService
    {
        private readonly RssParser parser;
        private readonly string rssUrl;

        public FeedService(string rssUrl)
        {
            parser = new RssParser();
            this.rssUrl = rssUrl;
        }

        public List<FeedItem> GetFeedItems()
        {

            string feed = "";
            using (var client = new HttpClient())
            {
                try
                {
                    feed = client.GetStringAsync(rssUrl).Result;
                }
                catch
                {
                    return new List<FeedItem>(0);
                }
            }

            var items = this.parser.Parse(feed).ToList();
            var resultList = new List<FeedItem>(items.Count);
            foreach (var item in items)
            {
                var feedItem = new FeedItem(item.Title, item.ImageUrl, item.Content, item.Summary, item.FeedUrl);
                resultList.Add(feedItem);
            }
            return resultList;
        }

    }
}