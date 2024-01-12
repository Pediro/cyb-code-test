using cyb_code_test.Models;
using cyb_code_test.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace cyb_code_test_tests
{
    public class DisneyApiServiceTests
    {
        DisneyCharacterApiService disneyCharacterApiService;

        [SetUp]
        public void Init()
        {
            var configurationDictionary = new Dictionary<string, string>
            {
                { "DisneyCharacterJsonPath", "C:\\Local\\cyb-code-test\\cyb-code-test\\Data\\disney-characters.json" }
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();
            disneyCharacterApiService = new(configuration);
        }
        [Test]
        public void FetchDisneyCharacters_ReturnsAchilles_True()
        {
            DisneyCharacter achilles = disneyCharacterApiService.FetchByPosition(0);

            DisneyCharacter expected = new()
            {
                Id = 112,
                Films = new List<string> { "Hercules (film)" },
                ShortFilms = new List<string>(),
                TvShows = new List<string> { "Hercules (TV series)" },
                VideoGames = new List<string> { "Kingdom Hearts III" },
                ParkAttractions = new List<string>(),
                Allies = new List<string>(),
                Enemies = new List<string>(),
                SourceUrl = "https://disney.fandom.com/wiki/Achilles_(Hercules)",
                Name = "Achilles",
                ImageUrl = "https://static.wikia.nocookie.net/disney/images/d/d3/Vlcsnap-2015-05-06-23h04m15s601.png",
                CreatedAt = DateTime.Parse("2021-04-12T01:31:30.547Z").ToUniversalTime(),
                UpdatedAt = DateTime.Parse("2021-12-20T20:39:18.033Z").ToUniversalTime(),
                Url = "https://api.disneyapi.dev/characters/112"
            };

            //Json comparison so I can avoid making a object compare override method
            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(achilles));
        }

        [Test]
        public void ListDisneyCharacters_ReturnsQueriedAmount_True()
        {
            List<DisneyCharacter> response = disneyCharacterApiService.List(1, 10);

            Assert.AreEqual(10, response.Count);
        }
    }
}
