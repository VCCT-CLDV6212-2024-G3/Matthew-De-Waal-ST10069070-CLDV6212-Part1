using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System.Configuration;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetail.Models.Tables
{
    public class CustomerProfileTable : TableStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerProfileTable() : 
            base(DataStorage.Configuration?.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), 
                "CustomerProfile", 
                new string[] {"UserName", "FirstName", "LastName", "Password"}) { }
    }
}
