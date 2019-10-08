namespace WykoBot
{
    public class FeedItem
    {
        public int HashCode { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }

        public FeedItem(string title, string imageUrl, string content, string summary, string url)
        {
            HashCode = (title + content).GetHashCode();
            Title = title;
            ImageUrl = imageUrl;
            Content = content;
            Summary = summary;
            Url = url;

        }
    }
}