using angular_cyb_code_test.Models;

namespace angular_cyb_code_test.Interfaces.Operations
{
    public interface IGuessTheCharacterOperations
    {
        Task<List<Question>> FetchGameDataAsync();
        List<string> FetchIncorrectAnswers(int numOfCharacters, DisneyCharacter character);
        List<Result> CheckAnswers(List<Question> submittedAnswers);
    }
}