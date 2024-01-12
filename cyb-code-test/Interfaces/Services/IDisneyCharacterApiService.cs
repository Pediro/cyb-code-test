using cyb_code_test.Models;

namespace cyb_code_test.Interfaces.Services
{
    public interface IDisneyCharacterApiService
    {
        int Count();
        DisneyCharacter FetchByPosition(int index);
        List<DisneyCharacter> List(int page, int pageSize);
    }
}