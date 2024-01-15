using angular_cyb_code_test.Models;

namespace angular_cyb_code_test.Interfaces.Services
{
    public interface IDisneyCharacterApiService
    {
        int Count();
        DisneyCharacter FetchByPosition(int index);
        DisneyCharacter? FetchById(int id);
        List<DisneyCharacter> List(int page, int pageSize);
    }
}