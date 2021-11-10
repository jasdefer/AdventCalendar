using AdventCalendarWebApp.Helper.TimeProvider;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Helper.Middleware;

public class StatisticLogger
{
    private readonly ILogger<StatisticLogger> logger;
    private readonly RequestDelegate _next;
    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly ITimeProvider timeProvider;

    public StatisticLogger(ILogger<StatisticLogger> logger,
        RequestDelegate next,
        AzureHelper azureHelper,
        IConfiguration configuration,
        ITimeProvider timeProvider)
    {
        this.logger = logger;
        _next = next;
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.timeProvider = timeProvider;
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
        var requestedTimestamp = timeProvider.Now();
        var baseUrl = requestedUrl;
        var arguments = string.Empty;
        if (baseUrl.Contains('?'))
        {
            var urlSegements = requestedUrl.Split('?', 2);
            baseUrl = urlSegements[0];
            arguments = urlSegements[1];
        }
        var httpRequestLog = new HttpRequestLog()
        {
            RequestTimestamp = requestedTimestamp,
            Method = context.Request.Method,
            BaseUrl = baseUrl,
            Arguments = arguments,
            UserId = userId,
            PartitionKey = userId,
            RowKey = Guid.NewGuid().ToString()
        };
        await azureHelper.AddObjectAsync(configuration["StorageData:RequestTableName"], httpRequestLog);
    }
}
