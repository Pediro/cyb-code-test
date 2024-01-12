using cyb_code_test.Models;
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
            List<Question> questions = _guessTheCharacterOperations.FetchGameData();

            Assert.AreEqual(questions.Count(), 10);
            int totalNumberOfAnswers = questions.Select(q => q.Answers.Count()).Sum();
            Assert.AreEqual(totalNumberOfAnswers, 40);

            foreach (Question question in questions)
            {
                DisneyCharacter? character = _disneyCharacterApiService.FetchById(question.Id);
                List<string> filmsAndTvShows = character.Films;
                filmsAndTvShows.AddRange(character.TvShows);

                Assert.IsTrue(question.Answers.Count == question.Answers.Distinct().Count()); //There shouldn't be any duplicates
                Assert.IsTrue(question.Answers.Intersect(filmsAndTvShows).Count() > 0);//A correct answer should in the options
            }
        }
    }
}
