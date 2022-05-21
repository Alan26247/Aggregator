using Aggregator.Models.Aggregator;
using System.Collections.Generic;
using System.Xml;

namespace Aggregator.Services.Aggregator
{
    /// <summary>
    /// Конвертор RSS из XML формата в  объект Rss
    /// </summary>
    class ConvertorXmlToRssObject
    {
        /// <summary>
        /// Конвертирует RSS из XML формата в  объект Rss
        /// </summary>
        /// <param name="xmlRss">RSS в формате XML</param>
        /// <returns></returns>
        public Rss Convert(string xmlRss)
        {
            Rss returnRss = new Rss();

            // загружаем xml
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlRss);

            // получаем корневой элемент
            XmlNode root = xmlDocument["rss"];
            if (root == null) return returnRss;

            // получаем елемент channel
            XmlNode channel = root["channel"];
            if (root == null) return returnRss;

            // заполняем данными объект rss
            if (channel["title"] != null) returnRss.Channel.Title = channel["title"].InnerText;
            if (channel["description"] != null) returnRss.Channel.Description = channel["description"].InnerText;
            if (channel["link"] != null) returnRss.Channel.Link = channel["link"].InnerText;

            // создаем список новостей и заполняем его
            List<News> listNews = new List<News>();
            foreach (XmlElement element in channel.ChildNodes)
            {
                if (element.Name != "item") continue;

                News news = new News();

                if (element["title"] != null) news.Title = element["title"].InnerText;
                if (element["description"] != null) news.Description = element["description"].InnerText;
                if (element["link"] != null) news.Link = element["link"].InnerText;
                if (element["pubDate"] != null) news.PubDate = element["pubDate"].InnerText;

                listNews.Add(news);
            }

            // конвертируем список в массив и передаем его возвращаемый объект
            returnRss.News = listNews.ToArray();

            return returnRss;
        }
    }
}

