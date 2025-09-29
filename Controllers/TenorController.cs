using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TenorApiProject.Models;
using TenorApiProject.Services;

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
            string apiKey = "AIzaSyD05UWDvF-6wJA8F7lB76BxXEnH4spmPjg";

            try
            {
                //No search term entered
                if (string.IsNullOrWhiteSpace(query))
                {
                    ViewBag.Error = "<strong>⚠️</strong> Please enter a keyword to search for GIFs.";

                    return View("Index", new List<TenorGifModel>());
                }

                // …call API here…
                if (string.IsNullOrEmpty(apiKey))
                {
                    ViewBag.Error = "<strong>🔑</strong> API key missing.";
                    return View("Index", new List<TenorGifModel>()); // empty list
                }

                // Safe to use query now
                var encodedQuery = Uri.EscapeDataString(query);
                var url = $"https://tenor.googleapis.com/v2/search?q={encodedQuery}&key={apiKey}&limit=10"; 

                var gifs = await _tenorService.SearchGifsAsync(query);

                if (gifs == null || !gifs.Any())
                {
                    ViewBag.Error = "<strong>😕</strong> No GIFs found.";
                    return View("Index", new List<TenorGifModel>());
                }

                return View("Index", gifs);
            }

            catch (Exception ex)
            {
                ViewBag.Error = $"Unexpected error: {ex.Message}";
                return View("Index", new List<TenorGifModel>());
            }
        }
    }
}
