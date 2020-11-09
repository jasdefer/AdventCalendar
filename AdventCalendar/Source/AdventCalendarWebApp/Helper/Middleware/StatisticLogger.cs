using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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
        private const string GetLogFileSuffix = "getlogs.csv";
        private const string PostLogFileSuffix = "postlogs.csv";
        public const string LogDir = "Logs/";


        public StatisticLogger(ILogger<StatisticLogger> logger, RequestDelegate next)
        {
            this.logger = logger;
            _next = next;
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
        }

        public static string GetGetLogFileName(DateTime date)
        {
            var fileName = $"{LogDir}{date.Date:yyyyMMdd}-{GetLogFileSuffix}";
            return fileName;
        }

        public static string GetPostLogFileName(DateTime date)
        {
            var fileName = $"{LogDir}{date.Date:yyyyMMdd}-{PostLogFileSuffix}";
            return fileName;
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
            var userId = context.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                userId = Guid.NewGuid().ToString();
                context.Session.SetString("UserId", userId);
            }
            var requestedUrl = UriHelper.GetDisplayUrl(context.Request);
            var now = DateTime.UtcNow;
            string message;
            string fileName;
            if (context.Request.Method == "GET")
            {
                message = $"{now}\t{userId}\t{requestedUrl}\r\n";
                fileName = GetGetLogFileName(now);
            }
            else if (context.Request.Method == "POST")
            {
                var formContent = string.Join('\t', context.Request.Form.Where(x => x.Key != "__RequestVerificationToken").Select(x => string.Join('\t', x.Value)));
                message = $"{now}\t{userId}\t{requestedUrl}\t{formContent}\r\n";
                fileName = GetPostLogFileName(now);
            }
            else
            {
                return;
            }
            await File.AppendAllTextAsync(fileName, message);
        }
    }
}