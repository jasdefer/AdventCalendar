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
            using var client = new HttpClient();
            var query = $"https://de.wikipedia.org/w/api.php?action=query&format=json&titles={keyword}&prop=extracts&explaintext";
            var response = await client.GetAsync(query);
            var json = await response.Content.ReadAsStringAsync();
            File.WriteAllText("test.json", json);
            var content = JsonSerializer.Deserialize<Root>(json);
            return content.query.pages.Single().Value.extract;
        }
    }

    public record Root(string batchcomplete, Query query);
    public record Query(Dictionary<string, Page> pages);
    public record Page(int pageid, string title, string extract);
}
