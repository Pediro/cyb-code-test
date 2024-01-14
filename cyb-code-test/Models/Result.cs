namespace cyb_code_test.Models
{
    public class Result
    {
        public string CharacterName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public string SelectedAnswer { get; set; }
        public List<string> AcceptedAnswers { get; set; }
    }
}
