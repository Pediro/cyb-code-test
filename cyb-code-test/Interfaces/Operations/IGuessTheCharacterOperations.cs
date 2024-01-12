using cyb_code_test.Models;

namespace cyb_code_test.Interfaces.Operations
{
    public interface IGuessTheCharacterOperations
    {
        GameDataViewModel FetchGameData();
        List<string> FetchIncorrectAnswers(int numOfCharacters, DisneyCharacter character);
    }
}