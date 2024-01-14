using Newtonsoft.Json;

namespace cyb_code_test.Models
{
    public class DisneyCharacter
    {
        [JsonProperty("_id")]
        public int Id { get; set; }

        [JsonProperty("films")]
        public List<string> Films { get; set; }

        [JsonProperty("shortFilms")]
        public List<string> ShortFilms { get; set; }

        [JsonProperty("tvShows")]
        public List<string> TvShows { get; set; }

        private List<string> _filmsAndTvShows;
        [JsonIgnore]
        public List<string> FilmsAndTvShows
        {
            get
            {
                _filmsAndTvShows = Films;
                _filmsAndTvShows.AddRange(TvShows);

                _filmsAndTvShows = _filmsAndTvShows.Distinct().OrderBy(s => Guid.NewGuid()).ToList();

                return _filmsAndTvShows;
            }
        }

        [JsonProperty("videoGames")]
        public List<string> VideoGames { get; set; }

        [JsonProperty("parkAttractions")]
        public List<string> ParkAttractions { get; set; }

        [JsonProperty("allies")]
        public List<string> Allies { get; set; }

        [JsonProperty("enemies")]
        public List<string> Enemies { get; set; }

        [JsonProperty("sourceUrl")]
        public string SourceUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }


    }
}
