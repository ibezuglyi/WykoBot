namespace WykoBot
{
    public class CacheItem
    {
        public FeedItem FeedItem { get; set; }
        public string UrlToSend { get; set; }

        public CacheItem(FeedItem feedItem, string urlToSend)
        {
            FeedItem = feedItem;
            UrlToSend = urlToSend;
        }
    }
}