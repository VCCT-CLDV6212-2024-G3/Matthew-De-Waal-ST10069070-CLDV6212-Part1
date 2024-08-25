using Azure.Storage.Blobs;

namespace ABCRetail.Models.BlobStorage
{
    public class ProductImageStorage : BlobStorageClass
    {
        public ProductImageStorage() : base(DataStorage.Configuration.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), "product-images") { }
    }
}
