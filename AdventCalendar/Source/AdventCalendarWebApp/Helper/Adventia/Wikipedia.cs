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
        public record SingleArticleRoot(string batchcomplete, SingleArticleQuery query);
        public record SingleArticleQuery(Dictionary<string, SingleArticlePage> pages);
        public record SingleArticlePage(int pageid, string title, string extract, IReadOnlyCollection<SingleArticleCategory> categories);
        public record SingleArticleCategory(int ns, string title);

        public static async Task<SingleArticlePage> GetTextAsync(string keyword)
        {
            var query = $"https://de.wikipedia.org/w/api.php?action=query&format=json&exintro&titles={keyword}&prop=extracts|categories&explaintext";
            using var client = new HttpClient();
            var response = await client.GetAsync(query);
            var json = await response.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<SingleArticleRoot>(json);
            return content.query.pages.Single().Value;
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

        public record RandomArticleRoot(string batchcomplete, RandomArticleQuery query);
        public record RandomArticleQuery(IReadOnlyCollection<RandomArticlePage> random);
        public record RandomArticlePage(int id, string title, int ns);

        public static async Task<IReadOnlyList<string>> GetRandomTitle(int number)
        {
            var query = $"https://de.wikipedia.org/w/api.php?action=query&format=json&list=random&rnlimit={number}&rnnamespace=0";
            using var client = new HttpClient();
            var response = await client.GetAsync(query);
            var json = await response.Content.ReadAsStringAsync();
            var content = JsonSerializer.Deserialize<RandomArticleRoot>(json);
            var titles = content.query.random.Select(x => x.title).ToArray();
            return titles;
        }

        public static string GetRandomKeyword(int seed)
        {
            var random = new Random(seed);
            var keyword = keywords[random.Next(0, keywords.Count)];
            return keyword;
        }
    }
}
