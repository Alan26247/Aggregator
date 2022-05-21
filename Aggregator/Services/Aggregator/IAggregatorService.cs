using Aggregator.Models.Aggregator;

namespace Aggregator.Services.Aggregator
{
    /// <summary>
    /// Данный интерфейс реализует работу с новостными каналами
    /// </summary>
    public interface IAggregatorService
    {
        /// <summary>
        /// Данный метод возвращает полный список новостей канала
        /// </summary>
        /// <returns>При успешном выполнении возвращает массив новостей иначе пустой массив</returns>
        public Rss GetNewsList();



        /// <summary>
        /// Данный метод возвращает список новостей согласно поиску в заголовке
        /// </summary>
        /// <param name="findStr">Строка поиска</param>
        /// <returns>При успешном выполнении возвращает массив новостей иначе пустой массив</returns>
        public Rss GetNewsList(string findStr);



        /// <summary>
        /// Обновляет новостную ленту канала.
        /// </summary>
        /// <param name="rss">Данные RSS</param>
        public void UpdateChannel(Rss rss);



        /// <summary>
        /// Очищает базу данных.
        /// </summary>
        public void Clear();
    }
}
