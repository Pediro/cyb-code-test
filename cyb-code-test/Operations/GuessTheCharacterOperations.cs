using cyb_code_test.Interfaces.Operations;
using cyb_code_test.Interfaces.Services;
using cyb_code_test.Models;
using System.Threading.Tasks;

namespace cyb_code_test.Operations
{
    public class GuessTheCharacterOperations : IGuessTheCharacterOperations
    {
        private readonly IDisneyCharacterApiService _disneyCharacterApiService;

        public GuessTheCharacterOperations(IDisneyCharacterApiService disneyCharacterApiService)
        {
            _disneyCharacterApiService = disneyCharacterApiService;
        }

        public List<Question> FetchGameData()
        {
            int numOfCharacters = _disneyCharacterApiService.Count();

            //Generating a list of random unique indexes to be used to fetch random disney characters
            Random rand = new();
            IEnumerable<int> indexes = Enumerable.Range(0, numOfCharacters)
                                         .Select(i => new Tuple<int, int>(rand.Next(numOfCharacters), i))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2)
                                         .Take(30);

            List<Question> questions = new();
            foreach (int index in indexes)
            {
                DisneyCharacter questionCharacter = _disneyCharacterApiService.FetchByPosition(index);

                //Some characters are not in any films and tv shows and some don't have an image. Ignore them
                if (questionCharacter.FilmsAndTvShows.Count == 0 || string.IsNullOrEmpty(questionCharacter.ImageUrl))
                {
                    continue;
                }

                string correctAnswer = questionCharacter.FilmsAndTvShows.First();
                List<string> answers = FetchIncorrectAnswers(numOfCharacters, questionCharacter);
                answers.Add(correctAnswer);

                //Order the answers by a random guid to randomise the list, ensuring the correct answer isn't always on the same position
                answers = answers.OrderBy(s => Guid.NewGuid()).ToList();

                questions.Add(new Question()
                {
                    CharacterName = questionCharacter.Name,
                    ImageUrl = questionCharacter.ImageUrl,
                    Id = questionCharacter.Id,
                    Answers = answers
                });

                if (questions.Count == 10) //I only need 10 of the 20 so stop when we have reached 10
                {
                    break;
                }
            }

            return questions;
        }

        public List<string> FetchIncorrectAnswers(int numOfCharacters, DisneyCharacter character)
        {
            int take = 10;

            int pageCount = numOfCharacters / take;
            int index = new Random().Next(1, pageCount);
            var apiResponse = _disneyCharacterApiService.List(index, take);

            //To try and avoid situations where we get answers similar to each other such as DuckTales and DuckTales (2017 series)
            //We are avoiding this by only returning the first film or tv show
            List<string> incorrectAnswers = new();
            foreach (DisneyCharacter item in apiResponse)
            {
                //Don't get answers from the character in the question
                if (item.Id == character.Id)
                {
                    continue;
                }

                if (item.FilmsAndTvShows.Count == 0)
                {
                    continue;
                }

                string selectedFilmOrTvShow = item.FilmsAndTvShows.First();

                // Avoid adding the same film or tv show into answers
                // Also avoiding characters that might share films or tv shows, which would cause confusion to the user as two answers could be valid
                if (character.FilmsAndTvShows.Contains(selectedFilmOrTvShow) || incorrectAnswers.Contains(selectedFilmOrTvShow))
                {
                    continue;
                }

                incorrectAnswers.Add(selectedFilmOrTvShow);

                if (incorrectAnswers.Count == 3)
                {
                    break;
                }
            }

            return incorrectAnswers;
        }

        public List<Result> CheckAnswers(List<Question> submittedAnswers)
        {
            if (submittedAnswers == null)
            {
                throw new ArgumentException("Missing submitted answers");
            }

            List<Result> results = new();
            foreach (Question question in submittedAnswers)
            {
                DisneyCharacter? character = _disneyCharacterApiService.FetchById(question.Id);

                if (character == null)
                {
                    throw new Exception("Failed to find Disney character");
                }

                results.Add(new Result
                {
                    AcceptedAnswers = character.FilmsAndTvShows,
                    CharacterName = question.CharacterName,
                    SelectedAnswer = question.SelectedAnswer,
                    IsCorrectAnswer = character.FilmsAndTvShows.Contains(question.SelectedAnswer),
                    ImageUrl = question.ImageUrl,
                });
            }

            return results;
        }
    }
}
