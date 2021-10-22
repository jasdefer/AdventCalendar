using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages.Misc.Statistics
{
    public class NumberOfRequestsModel : PageModel
    {
        public record UrlRequest(string BaseUrl, int NumberOfRequests);

        private readonly AzureHelper azureHelper;

        public NumberOfRequestsModel(AzureHelper azureHelper)
        {
            this.azureHelper = azureHelper;
        }

        public IReadOnlyList<UrlRequest> UrlRequests { get; set; }

        public void OnGet()
        {
            var table = azureHelper.GetTableReference("AdventCalendarHttpRequests");
            var query = new TableQuery<HttpRequestLog>();
            var result = table.ExecuteQuery(query);
            UrlRequests = result.GroupBy(x => x.BaseUrl).Select(y => new UrlRequest(y.Key, y.Count())).ToArray();
        }
    }
}