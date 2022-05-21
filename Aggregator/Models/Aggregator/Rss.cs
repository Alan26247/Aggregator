namespace Aggregator.Models.Aggregator
{
    public class Rss
    {
        public Channel Channel { get; set; }
        public News[] News { get; set; }

        public Rss()
        {
            Channel = new Channel();
        }
    }
}
