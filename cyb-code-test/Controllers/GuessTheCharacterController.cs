using cyb_code_test.Interfaces.Services;
using cyb_code_test.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace cyb_code_test.Controllers
{
    public class GuessTheCharacterController : Controller
    {
        private readonly IDisneyCharacterApiService _disneyCharacterApiService;

        public GuessTheCharacterController(IDisneyCharacterApiService disneyCharacterApiService)
        {
            _disneyCharacterApiService = disneyCharacterApiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GameAsync()
        {
            return View();
        }

    }
}
