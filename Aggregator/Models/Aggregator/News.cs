using System.Text.Json.Serialization;

namespace Aggregator.Models.Aggregator
{
    public class News
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? PubDate { get; set; }

        [JsonIgnore]
        public Channel Channel { get; set; }             // зависимость
    }
}
