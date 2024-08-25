using Azure.Storage.Files.Shares;

namespace ABCRetail.Models.FileShares
{
    public abstract class FileStorageClass
    {
        private readonly string? connectionString;
        private readonly string? containerName;

        public FileStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        public async void AddItem(string name, byte[] bytes)
        {
            ShareFileClient client = new ShareFileClient(connectionString, containerName, name);
            client.Create(20000);
            client.UploadAsync(new MemoryStream(bytes));
        }

        public async void RemoveItem(string name)
        {
            ShareFileClient client = new ShareFileClient(connectionString, containerName, name);
            client.DeleteAsync();
        }
    }
}
