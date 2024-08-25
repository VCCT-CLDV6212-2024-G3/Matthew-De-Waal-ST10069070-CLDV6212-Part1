
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace ABCRetail.Models.BlobStorage
{
    public abstract class BlobStorageClass
    {
        private readonly string? connectionString;
        private readonly string? containerName;

        public BlobStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        public Stream this[string name]
        {
            get
            {
                BlobClient client = new BlobClient(connectionString, containerName, name);

                return client.DownloadContentAsync().Result.Value.Content.ToStream();
            }
            set
            {
                BlobClient client = new BlobClient(connectionString, containerName, name);
                client.UploadAsync(value);
            }
        }

        public void AddItem(string name, byte[] bytes)
        {
            BlobClient client = new BlobClient(connectionString, containerName, name);
            BinaryData binaryData = new BinaryData(bytes);
            client.UploadAsync(binaryData);
        }

        public void RemoveItem(string name)
        {
            BlobClient client = new BlobClient(connectionString, containerName, name);
            client.DeleteAsync();
        }

        public void DeleteAll()
        {
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();

            foreach(BlobItem item in blobs)
            {
                client.DeleteBlob(item.Name);
            }
        }

        public StringCollection RecieveData()
        {
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();
            StringCollection collection = new StringCollection();

            foreach(BlobItem item in blobs)
            {
                collection.Add($"{item.Name}:{item.Name.Substring(0, item.Name.LastIndexOf('.'))}");
            }

            return collection;
        }

        public StringCollection RecieveData(string path)
        {
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);
            var blobs = client.GetBlobs();
            StringCollection collection = new StringCollection();

            foreach (BlobItem item in blobs)
            {
                if($"/{item.Name}/".Contains(path))
                    collection.Add($"{item.Name}:{item.Name.Substring(0, item.Name.LastIndexOf('.'))}");
            }

            return collection;
        }

        public string GetFullPath(string name)
        {
            BlobContainerClient client = new BlobContainerClient(connectionString, containerName);

            return $"{client.Uri.ToString()}/{name}";
        }
    }
}
