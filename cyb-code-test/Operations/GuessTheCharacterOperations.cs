using cyb_code_test.Interfaces.Services;
using cyb_code_test.Models;
using System.Threading.Tasks;

namespace cyb_code_test.Operations
{
    public class GuessTheCharacterOperations
    {
        private readonly IDisneyCharacterApiService _disneyCharacterApiService;

        public GuessTheCharacterOperations(IDisneyCharacterApiService disneyCharacterApiService)
        {
            _disneyCharacterApiService = disneyCharacterApiService;
        }

        public async Task<GameDataViewModel> FetchGameData()
        {
            int numOfCharacters = _disneyCharacterApiService.Count();

            //Generating a list of random unique indexes to be used to fetch random disney characters
            Random rand = new();
            IEnumerable<int> indexes = Enumerable.Range(0, 20)
                                         .Select(i => new Tuple<int, int>(rand.Next(numOfCharacters), i))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2);

            //The following requests all the data from the disney api asynchronously then wait for all the threads to finish at the end
            List<Question> questions = new();

            //Trigger all requests to run
            foreach (int index in indexes)
            {
                DisneyCharacter questionCharacter = _disneyCharacterApiService.FetchByPosition(index);

                List<string> filmsAndTvShows = questionCharacter.Films;
                filmsAndTvShows.AddRange(questionCharacter.TvShows);

                //Some characters are not in any films and tv shows. Ignore them
                if (filmsAndTvShows.Count == 0)
                {
                    continue;
                }

                //Randomising the list of films and tv shows so we get a variety of films and tv shows as the correct answer
                filmsAndTvShows = filmsAndTvShows.OrderBy(s => Guid.NewGuid()).ToList();

                string correctAnswer = filmsAndTvShows.First();
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

            return new GameDataViewModel()
            {
                Questions = questions
            };
        }

        public List<string> FetchIncorrectAnswers(int numOfCharacters, DisneyCharacter character)
        {
            int pageCount = numOfCharacters % 6;
            int index = new Random().Next(1, pageCount);
            var apiResponse = _disneyCharacterApiService.List(index, 6);

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

                List<string> filmsAndTvShows = item.Films;
                filmsAndTvShows.AddRange(item.TvShows);

                if (filmsAndTvShows.Count == 0) { 
                    continue; 
                }

                if (filmsAndTvShows.Count > 1)
                {
                    filmsAndTvShows = filmsAndTvShows.OrderBy(s => Guid.NewGuid()).ToList(); //Randomise order
                }

                incorrectAnswers.Add(filmsAndTvShows.First());

                if (incorrectAnswers.Count == 3)
                {
                    break;
                }
            }

            return incorrectAnswers;
        }
    }
}
