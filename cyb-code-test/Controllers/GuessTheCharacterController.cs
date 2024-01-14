﻿using cyb_code_test.Interfaces.Operations;
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

        public IActionResult Error()
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
            Console.WriteLine("Begin fetching game data");
            try
            {
                return new JsonResult(_guessTheCharacterOperations.FetchGameData());
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
