using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Helper;

public class AzureHelper
{
    private readonly string ConnectingString;
    private readonly ILogger<AzureHelper> logger;

    public AzureHelper(IConfiguration configuration,
        ILogger<AzureHelper> logger)
    {
        ConnectingString = configuration["AzureStorageConnectingString"];
        this.logger = logger;
    }

    public CloudTable GetTableReference(string tableName, bool createIfNotExists = true)
    {
        try
        {
            var account = CloudStorageAccount.Parse(ConnectingString);
            var client = account.CreateCloudTableClient();

            var table = client.GetTableReference(tableName);

            if (createIfNotExists)
            {
                table.CreateIfNotExists();
            }
            return table;
        }
        catch (Exception e)
        {
            logger.LogError($"Cannot get table reference for the table '{tableName}'. {e.Message}");
            throw;
        }
    }

    public async Task AddObjectAsync<T>(CloudTable table, T value) where T : ITableEntity
    {
        if (table == null)
        {
            throw new ArgumentNullException(nameof(table));
        }

        TableOperation operation = TableOperation.InsertOrReplace(value);
        await table.ExecuteAsync(operation);
    }

    public async Task AddObjectAsync<T>(string tableName, T value) where T : ITableEntity
    {
        try
        {
            var table = GetTableReference(tableName);
            await AddObjectAsync(table, value);
        }
        catch (Exception e)
        {
            logger.LogError($"Cannot add object to table storage '{tableName}'", e);
        }
    }
}
