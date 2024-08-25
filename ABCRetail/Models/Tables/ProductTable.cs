using Azure.Data.Tables;

namespace ABCRetail.Models.Tables
{
    public class ProductTable : TableStorageClass
    {
        public ProductTable() : 
            base(DataStorage.Configuration?.GetSection("ConnectionStrings").GetValue<string>("AzureStorage"), 
                "Product", 
                new string[] {"ProductName", "Price", "Category", "TotalStock", "ImagePath"}) { }
    }
}
