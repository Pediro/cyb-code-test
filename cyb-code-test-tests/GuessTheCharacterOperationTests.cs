using cyb_code_test.Operations;
using cyb_code_test.Services;
using Microsoft.Extensions.Configuration;

namespace cyb_code_test_tests
{
    public class GuessTheCharacterOperationTests
    {
        DisneyCharacterApiService _disneyCharacterApiService;
        GuessTheCharacterOperations _guessTheCharacterOperations;

        [SetUp]
        public void Init()
        {
            var configurationDictionary = new Dictionary<string, string>
            {
                { "DisneyCharacterJsonPath", "C:\\Local\\cyb-code-test\\cyb-code-test\\Data\\disney-characters.json" }
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationDictionary).Build();

            _disneyCharacterApiService = new(configuration);
            _guessTheCharacterOperations = new(_disneyCharacterApiService);
        }

        [Test]
        public async Task FetchGameDataAsync_ShouldProduceGameData_TrueAsync()
        {
            var gameDataViewModel = await _guessTheCharacterOperations.FetchGameData();

            Assert.AreEqual(gameDataViewModel.Questions.Count(), 10);
            int totalNumberOfAnswers = gameDataViewModel.Questions.Select(q => q.Answers.Count()).Sum();
            Assert.AreEqual(totalNumberOfAnswers, 40);
        }
    }
}
