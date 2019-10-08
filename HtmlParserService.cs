using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WykoBot
{
    internal class HtmlParserService
    {
        private readonly HttpClient client;

        public HtmlParserService()
        {
            client = new HttpClient();
        }
        internal List<CacheItem> ParseHtmls(List<CacheItem> notificationItems)
        {
            foreach(var item in notificationItems)
            {
                using (var downloadedStream = client.GetStreamAsync(item.FeedItem.Url).Result)
                {
                    HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.Load(downloadedStream);
                    var nodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='fix-tagline']");
                    foreach(var node in nodes)
                    {
                        var ahrefNodes = node.SelectNodes(".//a");
                        if (ahrefNodes.Count > 1)
                        {
                            item.UrlToSend = ahrefNodes[1].GetAttributeValue("href", null);
                        }

                    }
                }
                
            }

            return notificationItems;
            
        }
    }
}