﻿using Newtonsoft.Json;

namespace cyb_code_test.Models
{
    public class DisneyCharacterApiResponse<T>
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
