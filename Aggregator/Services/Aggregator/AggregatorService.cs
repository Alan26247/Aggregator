using Aggregator.Data.Aggregator;
using Aggregator.Models.Aggregator;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Aggregator.Services.Aggregator
{
    /// <inheritdoc />
    public class AggregatorService : IAggregatorService
    {
        public AggregatorService(IConfiguration configuration)
        {
            db = new MySqlDbContext(configuration);
        }

        private readonly MySqlDbContext db; // контекст данных



        // ============================ INewsService =====================================

        /// <inheritdoc />
        public Rss GetNewsList()
        {
            Rss rss = new Rss();
            // сначало смотрим есть ли канал в БД
            Channel[] channels = db.Channels.ToArray();
            if (channels.Length > 0)
            {
                rss.Channel = channels.First();
                rss.News = (from news in db.News
                           where news.Channel == channels.First()
                            select news).ToArray();
            }

            return rss;
        }

        /// <inheritdoc />
        public Rss GetNewsList(string findStr)
        {
            Rss rss = new Rss();

            // сначало смотрим есть ли канал в БД
            Channel[] channels = db.Channels.ToArray();
            if (channels.Length > 0)
            {
                rss.Channel = channels.First();

                // ищем в новостях в названиях совпадение
                List<News> listNews = new List<News>(); 

                // получаем весь список
                rss.News = (from news in db.News
                            where news.Channel == channels.First()
                            select news).ToArray();

                // приводим строку поиска к нижнему регистру
                findStr = findStr.ToLower();

                foreach (News news in rss.News)
                {
                    // приводим к нижнему регистру
                    news.Title = news.Title.ToLower();

                    if (news.Title.Contains(findStr)) listNews.Add(news);
                }

                rss.News = listNews.ToArray();
            }

            return rss;
        }

        /// <inheritdoc />
        public async void UpdateChannel(Rss rss)
        {
            // удаляем каналы из БД
            Clear();

            // добавляем новый канал
            db.Channels.Add(rss.Channel);
            await db.SaveChangesAsync();

            // наполняем новым списком
            Channel channel = db.Channels.Single(_channel => _channel.RSS == rss.Channel.RSS);
            foreach (News news in rss.News)
            {
                news.Channel = channel;
                db.News.Add(news);
            }

            await db.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async void Clear()
        {
            foreach(Channel channel in db.Channels)
            {
                db.Channels.Remove(channel);
            }
            
            await db.SaveChangesAsync();
        }
    }
}
