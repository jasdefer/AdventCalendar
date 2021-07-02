using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventCalendarWebApp.Helper.Adventia
{
    public static class Wikipedia
    {
        public static async Task<string> GetTextAsync(string keyword)
        {
            var query = $"https://de.wikipedia.org/w/api.php?action=query&format=json&exintro&titles={keyword}&prop=extracts&explaintext";
            using var client = new HttpClient();
            var response = await client.GetAsync(query);
            var json = await response.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<Root>(json);
            return content.query.pages.Single().Value.extract;
        }

        private static readonly IReadOnlyList<string> keywords = new string[]
        {
            "basketball",
            "kellerassel",
            "haushund",
            "rathaus",
            "mathematik",
            "asteroid",
            "berlin",
            "arzt",
            "kamera",
            "gesang",
            "klavier",
            "kupfer",
            "eichen",
            "kontinent",
            "säuren",
            "buch",
            "schienbein",
            "politik",
            "wasser",
            "wetter",
            "architektur",
            "dinosaurier",
            "olympiade",
            "eisenbahn",
            "blumenstrauß"
        };

        public static async Task<string> GetRandomTitle()
        {
            var query = "https://de.wikipedia.org/w/api.php?action=query&format=json&generator=random&prop=extracts&explaintext";
            using var client = new HttpClient();
            Page page;
            do
            {
                var response = await client.GetAsync(query);
                var json = await response.Content.ReadAsStringAsync();
                var content = JsonSerializer.Deserialize<Root>(json);
                page = content.query.pages.Single().Value;
            }
            while (page.extract.Length < 50);
            return page.title;
        }


        public static string GetRandomKeyword(int seed)
        {
            var random = new Random(seed);
            var keyword = keywords[random.Next(0, keywords.Count)];
            return keyword;
        }
    }

    public record Root(string batchcomplete, Query query);
    public record Query(Dictionary<string, Page> pages);
    public record Page(int pageid, string title, string extract);
}
