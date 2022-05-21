using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aggregator.Models.Aggregator
{
    public class Channel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? RSS { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        [JsonIgnore]
        public List<News> News { get; set; }
    }
}
