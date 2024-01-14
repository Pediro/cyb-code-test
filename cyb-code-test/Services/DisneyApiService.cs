using cyb_code_test.Exceptions;
using cyb_code_test.Interfaces.Services;
using cyb_code_test.Models;
using Newtonsoft.Json;

namespace cyb_code_test.Services
{
    public class DisneyCharacterApiService : IDisneyCharacterApiService
    {
        private readonly IConfiguration _configuration;
        private readonly List<DisneyCharacter> _disneyCharactersDb;

        public DisneyCharacterApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _disneyCharactersDb = ReadFromJson();
        }

        public int Count()
        {
            return _disneyCharactersDb.Count;
        }

        public List<DisneyCharacter> List(int page, int pageSize)
        {
            int offset = (page - 1) * pageSize;

            List<DisneyCharacter> disneyCharacters = _disneyCharactersDb.Skip(offset).Take(pageSize).ToList();

            return disneyCharacters;
        }

        public DisneyCharacter FetchByPosition(int index)
        {
            DisneyCharacter disneyCharacter = _disneyCharactersDb.ElementAt(index);
            return disneyCharacter;
        }

        public DisneyCharacter? FetchById(int id)
        {
            return _disneyCharactersDb.FirstOrDefault(d => d.Id == id);
        }

        // Due to limitations in Disney api, where it kept returning 503 if I sent to many requests I've downloaded all the data into a json file to be used for this test
        private List<DisneyCharacter> ReadFromJson()
        {
            string? disneyCharacterJsonPath = _configuration.GetValue<string>("DisneyCharacterJsonPath");

            if (string.IsNullOrEmpty(disneyCharacterJsonPath))
            {
                throw new EntityNotFoundException("Missing Disney file path");
            }

            using StreamReader reader = new(disneyCharacterJsonPath);
            DisneyCharacterApiResponse<List<DisneyCharacter>>? responseObject = JsonConvert.DeserializeObject<DisneyCharacterApiResponse<List<DisneyCharacter>>>(reader.ReadToEnd());

            if (responseObject == null)
            {
                throw new Exception("Failed to initalise the Disney database");
            }

            return responseObject.Data;
        }
    }
}
