using cyb_code_test.Interfaces.Operations;
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
            var gameDataViewModel = _guessTheCharacterOperations.FetchGameData();

            return View(gameDataViewModel);
        }

    }
}
