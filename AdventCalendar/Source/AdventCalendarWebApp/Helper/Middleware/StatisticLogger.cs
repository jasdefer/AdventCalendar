using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventCalendarWebApp.Helper.Middleware
{
    public class StatisticLogger
    {
        private readonly ILogger<StatisticLogger> logger;
        private readonly RequestDelegate _next;
        private readonly AzureHelper azureHelper;
        private const string GetLogFileSuffix = "getlogs.csv";
        private const string PostLogFileSuffix = "postlogs.csv";
        public const string LogDir = "Logs/";


        public StatisticLogger(ILogger<StatisticLogger> logger,
            RequestDelegate next,
            AzureHelper azureHelper)
        {
            this.logger = logger;
            _next = next;
            this.azureHelper = azureHelper;
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await AddLog(context);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Cannot log request");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private async Task AddLog(HttpContext context)
        {

            var userId = context.GetOrCreateUserId();
            var requestedUrl = UriHelper.GetDisplayUrl(context.Request);
            var requestedTimestamp = DateTime.UtcNow;
            var httpRequestLog = new HttpRequestLog()
            {
                RequestTimestamp = DateTime.UtcNow,
                Method = context.Request.Method,
                Url = requestedUrl,
                UserId = userId,
                PartitionKey = userId,
                RowKey = requestedTimestamp.Ticks.ToString()
            };
            await azureHelper.AddObjectAsync("AdventCalendarHttpRequests", httpRequestLog);
        }
    }
}