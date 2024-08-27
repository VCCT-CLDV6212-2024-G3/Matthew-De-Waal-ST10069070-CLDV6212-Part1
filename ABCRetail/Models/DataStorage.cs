using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files;
using System.Configuration;
using ABCRetail.Models.BlobStorage;
using ABCRetail.Models.FileShares;
using ABCRetail.Models.Queues;
using ABCRetail.Models.Tables;

namespace ABCRetail.Models
{
    public static class DataStorage
    {
        private static IConfiguration? configuration;
        private static BlobStorageClass? multimediaStorage;
        private static BlobStorageClass? productImageStorage;
        private static FileStorageClass? contractStorage;
        private static FileStorageClass? logStorage;
        private static QueueStorageClass? inventoryStorage;
        private static QueueStorageClass? transactionStorage;
        private static TableStorageClass? customerProfileTable;
        private static TableStorageClass? productTable;
        private static bool bInitialized;

        public static void Initialize(IConfiguration _configuration)
        {
            configuration = _configuration;

            multimediaStorage = new MultimediaStorage();
            productImageStorage = new ProductImageStorage();
            contractStorage = new ContractStorage();
            logStorage = new LogStorage();
            inventoryStorage = new InventoryStorage();
            transactionStorage = new TransactionStorage();
            customerProfileTable = new CustomerProfileTable();
            productTable = new ProductTable();

            bInitialized = true;
        }

        public static IConfiguration? Configuration => configuration;
        public static BlobStorageClass? MultimediaStorage => multimediaStorage;
        public static BlobStorageClass? ProductImageStorage => productImageStorage;
        public static FileStorageClass? ContractStorage => contractStorage;
        public static FileStorageClass? LogStorage => logStorage;
        public static QueueStorageClass? InventoryStorage => inventoryStorage;
        public static QueueStorageClass? TransactionStorage => transactionStorage;
        public static TableStorageClass? CustomerProfileTable => customerProfileTable;
        public static TableStorageClass? ProductTable => productTable;

        public static bool Initialized { get => bInitialized; }
    }
}
