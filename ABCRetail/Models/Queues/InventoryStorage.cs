using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;
using Azure.Storage.Queues.Models;

namespace ABCRetail.Models.Queues
{
    public class InventoryStorage : QueueStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryStorage() :
            base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"),
                "inventory") { }
    }
}
