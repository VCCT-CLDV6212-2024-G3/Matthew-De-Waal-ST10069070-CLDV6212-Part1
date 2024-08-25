using Azure.Storage.Blobs;

namespace ABCRetail.Models.BlobStorage
{
    public class ProductImageStorage : BlobStorageClass
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductImageStorage() : 
            base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), 
                "product-images") { }
    }
}
