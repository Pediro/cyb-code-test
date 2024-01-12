using cyb_code_test.Interfaces.Operations;
using cyb_code_test.Models;
using Microsoft.AspNetCore.Mvc;

namespace cyb_code_test.Controllers
{
    public class GuessTheCharacterController : Controller
    {
        private readonly IGuessTheCharacterOperations _guessTheCharacterOperations;

        public GuessTheCharacterController(IGuessTheCharacterOperations guessTheCharacterOperations)
        {
            _guessTheCharacterOperations = guessTheCharacterOperations;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Game()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FetchGameData()
        {
            return new JsonResult(_guessTheCharacterOperations.FetchGameData());
        }
    }
}
