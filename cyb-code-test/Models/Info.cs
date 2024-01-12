using Newtonsoft.Json;

namespace cyb_code_test.Models
{
    public class Info
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("previousPage")]
        public string? PreviousPage { get; set; }

        [JsonProperty("nextPage")]
        public string? NextPage { get; set; }
    }
}
