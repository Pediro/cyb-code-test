using Newtonsoft.Json;

namespace angular_cyb_code_test.Models
{
    public class Question
    {
        public string CharacterName { get; set; }
        public string ImageUrl { get; set; }
        public int Id { get; set; }
        public List<string> Answers { get; set; }
        public string? SelectedAnswer { get; set; }

        public Question() { }
    }
}
