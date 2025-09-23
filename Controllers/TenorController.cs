using Microsoft.AspNetCore.Mvc;
using TenorApiProject.Services;
using TenorApiProject.Models;

namespace TenorApiProject.Controllers
{
    public class TenorController : Controller
    {
        private readonly TenorApiService _tenorService;

        public TenorController(TenorApiService tenorService)
        {
            _tenorService = tenorService;
        }

        public IActionResult Index()
        {
            return View(new List<TenorGifModel>());
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            var gifs = await _tenorService.SearchGifsAsync(query);
            return View("Index", gifs);
        }
    }
}
