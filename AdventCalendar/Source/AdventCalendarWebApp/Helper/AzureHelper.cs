using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AdventCalendarWebApp.Helper
{
    public class AzureHelper
    {
        private string ConnectingString;
        private readonly ILogger<AzureHelper> logger;

        public AzureHelper(IConfiguration configuration,
            ILogger<AzureHelper> logger)
        {
            ConnectingString = configuration["AzureStorageConnectingString"];
            this.logger = logger;
        }

        public CloudTable GetTableReference(string tableName, bool createIfNotExists = true)
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
}