using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.Models.FileShares
{
    public class ContractStorage : FileStorageClass
    {
        public ContractStorage() : base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), "contracts")
        {

        }
    }
}
