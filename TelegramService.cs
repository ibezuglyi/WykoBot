using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace WykoBot
{
    public class TelegramService
    {
        private readonly TelegramBotClient botClient;
        private string chatFile;
        private HashSet<long> chatIds;

        public TelegramService(string token, string location)
        {
            chatFile = Path.Combine(Path.GetDirectoryName(location), "chats.bin");
            chatIds = DeserializeChats();
            botClient = new TelegramBotClient(token);
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        private HashSet<long> DeserializeChats()
        {
            if (!File.Exists(chatFile))
            {
                return new HashSet<long>();
            }
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(chatFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var fileChatIds = formatter.Deserialize(stream) as HashSet<long>;
                stream.Close();
                return fileChatIds;
            }
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "start_me")
            {
                chatIds.Add(e.Message.Chat.Id);
                SaveChatIds();
            }
        }

        private void SaveChatIds()
        {
            if (File.Exists(chatFile))
            {
                File.Delete(chatFile);
            }
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(chatFile, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, chatIds);
                stream.Close();
            }
        }

        public void SendNotifications(List<CacheItem> notifications)
        {
            foreach (var item in notifications)
                foreach (var chatId in chatIds)
                {
                    botClient.SendTextMessageAsync(chatId, $"[{item.FeedItem.Title}]({item.UrlToSend}) {item.FeedItem.Summary}", Telegram.Bot.Types.Enums.ParseMode.Markdown, disableNotification: true);
                }
        }
    }
}