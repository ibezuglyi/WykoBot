using System;
using System.Reflection;
using System.Timers;

namespace WykoBot
{
    class Program
    {
        private static FeedService feedService;
        private static CacheService cacheService;
        private static TelegramService senderService;
        private static HtmlParserService parserService;

        static void Main(string[] args)
        {
            feedService = new FeedService(Environment.GetEnvironmentVariable("rss_url"));
            parserService = new HtmlParserService();
            cacheService = new CacheService();
            senderService = new TelegramService(Environment.GetEnvironmentVariable("tele_token"), Assembly.GetExecutingAssembly().Location);


            var timer = new Timer();
            timer.Interval = 30000;
            timer.Elapsed += OnTimer;
            timer.Start();
            Console.ReadKey();
        }

        private static void OnTimer(object sender, ElapsedEventArgs e)
        {
            var items = feedService.GetFeedItems();
            var notificationItems = cacheService.CreateNewNotications(items);
            var notifications = parserService.ParseHtmls(notificationItems);
            senderService.SendNotifications(notifications);

        }
    }
}
