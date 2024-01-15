using angular_cyb_code_test.Models;
using angular_cyb_code_test.Operations;
using angular_cyb_code_test.Services;
using Microsoft.Extensions.Configuration;

namespace cyb_code_test_tests
{
    public class GuessTheCharacterOperationTests
    {
        DisneyCharacterApiService _disneyCharacterApiService;
        GuessTheCharacterOperations _guessTheCharacterOperations;

        //Setting up Disney Character data
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

        //Expecting to find 10 questions where each question has a valid answer and there are no duplicate answers in the available options
        [Test]
        public void FetchGameDataAsync_ShouldProduceGameData_True()
        {
            List<Question> questions = _guessTheCharacterOperations.FetchGameDataAsync();

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

        //Checking the logic can determine if the given answer is correct or not, expecting a correct answer
        [Test]
        public void CheckAnswers_ShouldReturnIsCorrectAnswer_True()
        {
            List<Question> questions = new List<Question>()
            {
                new Question()
                {
                    CharacterName = "Jack and Jill",
                    ImageUrl = "https://static.wikia.nocookie.net/disney/images/8/81/Jackandjill.jpg",
                    Id = 3310,
                    Answers = new List<string>() {
                        "The Lion Guard",
                        "Lilo & Stitch: The Series",
                        "Once Upon a Time",
                        "American Dragon: Jake Long"
                    },
                    SelectedAnswer = "Once Upon a Time"
                }
            };

            List<Result> results = _guessTheCharacterOperations.CheckAnswers(questions);

            Assert.AreEqual(results.First().IsCorrectAnswer, true);
        }

        //Checking the logic can determine if the given answer is correct or not, expecting a incorrect answer
        [Test]
        public void CheckAnswers_ShouldReturnIsCorrectAnswer_False()
        {
            List<Question> questions = new List<Question>()
            {
                new Question()
                {
                    CharacterName = "Jack and Jill",
                    ImageUrl = "https://static.wikia.nocookie.net/disney/images/8/81/Jackandjill.jpg",
                    Id = 3310,
                    Answers = new List<string>() {
                        "The Lion Guard",
                        "Lilo & Stitch: The Series",
                        "Once Upon a Time",
                        "American Dragon: Jake Long"
                    },
                    SelectedAnswer = "American Dragon: Jake Long"
                }
            };

            List<Result> results = _guessTheCharacterOperations.CheckAnswers(questions);

            Assert.AreEqual(results.First().IsCorrectAnswer, false);
        }
    }
}
