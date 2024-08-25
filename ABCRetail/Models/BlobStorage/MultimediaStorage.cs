using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files;
using System.Configuration;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.IO;
using System.Configuration.Internal;

namespace ABCRetail.Models.BlobStorage
{
    public class MultimediaStorage : BlobStorageClass
    {
        public MultimediaStorage() : base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), "multimedia") { }
    }
}
