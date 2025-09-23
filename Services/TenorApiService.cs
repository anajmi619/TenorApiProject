using System.Net.Http;
using System.Text.Json;
using TenorApiProject.Models;

namespace TenorApiProject.Services
{
    public class TenorApiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TenorApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["TenorApiKey"];
            _httpClient = httpClient;
        }

        public async Task<List<TenorGifModel>> SearchGifsAsync(string query)
        {
            var url = $"https://tenor.googleapis.com/v2/search?q={Uri.EscapeDataString(query)}&key={_apiKey}&limit=10";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var gifs = new List<TenorGifModel>();
            foreach (var result in doc.RootElement.GetProperty("results").EnumerateArray())
            {
                var media = result.GetProperty("media_formats").GetProperty("gif").GetProperty("url").GetString();
                var preview = result.GetProperty("media_formats").GetProperty("tinygif").GetProperty("url").GetString();
                gifs.Add(new TenorGifModel
                {
                    Title = result.GetProperty("content_description").GetString(),
                    Url = media,
                    PreviewUrl = preview
                });
            }

            return gifs;
        }
    }
}
