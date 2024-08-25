using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.Models.Queues
{
    public class TransactionStorage : QueueStorageClass
    {
        public TransactionStorage() : base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), "transactions")
        {

        }
    }
}
