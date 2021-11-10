using System.Net.Http;
using System.Text.Json;

namespace AdventCalendarWebApp.Helper.Adventia;

public static class Wikipedia
{
    public record SingleArticleRoot(string batchcomplete, SingleArticleQuery query);
    public record SingleArticleQuery(Dictionary<string, SingleArticlePage> pages);
    public record SingleArticlePage(int pageid, string title, string extract, IReadOnlyCollection<SingleArticleCategory> categories);
    public record SingleArticleCategory(int ns, string title);

    public static async Task<SingleArticlePage> GetTextAsync(string keyword)
    {
        var query = $"https://en.wikipedia.org/w/api.php?action=query&format=json&exintro&titles={keyword}&prop=extracts|categories&explaintext";
        using var client = new HttpClient();
        var response = await client.GetAsync(query);
        var json = await response.Content.ReadAsStringAsync();
        var content = JsonSerializer.Deserialize<SingleArticleRoot>(json);
        return content.query.pages.Single().Value;
    }

    public record RandomArticleRoot(string batchcomplete, RandomArticleQuery query);
    public record RandomArticleQuery(IReadOnlyCollection<RandomArticlePage> random);
    public record RandomArticlePage(int id, string title, int ns);

    public static async Task<IReadOnlyList<string>> GetRandomTitle(int number)
    {
        var query = $"https://en.wikipedia.org/w/api.php?action=query&format=json&list=random&rnlimit={number}&rnnamespace=0";
        using var client = new HttpClient();
        var response = await client.GetAsync(query);
        var json = await response.Content.ReadAsStringAsync();
        var content = JsonSerializer.Deserialize<RandomArticleRoot>(json);
        var titles = content.query.random.Select(x => x.title).ToArray();
        return titles;
    }
}
