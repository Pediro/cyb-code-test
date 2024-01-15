using angular_cyb_code_test.Interfaces.Operations;
using angular_cyb_code_test.Models;
using Microsoft.AspNetCore.Mvc;

namespace angular_cyb_code_test.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IGuessTheCharacterOperations _guessTheCharacterOperations;

        public ApiController(IGuessTheCharacterOperations guessTheCharacterOperations)
        {
            _guessTheCharacterOperations = guessTheCharacterOperations;
        }

        [HttpGet]
        public async Task<IActionResult> FetchGameDataAsync()
        {
            Console.WriteLine("Begin fetching game data");
            try
            {
                return new JsonResult(await _guessTheCharacterOperations.FetchGameDataAsync());
            }
            finally
            {
                Console.WriteLine("Completed fetching game data");
            }
        }

        [HttpPost]
        public IActionResult SubmitAnswers([FromBody] List<Question> submittedAnswers)
        {
            Console.WriteLine("Begin checking submitted answers");
            try
            {
                return new JsonResult(_guessTheCharacterOperations.CheckAnswers(submittedAnswers));
            }
            finally
            {
                Console.WriteLine("Completed checking submitted answers");
            }
        }
    }
}
