using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using System.Collections.Specialized;

namespace ABCRetail.Models.FileShares
{
    public abstract class FileStorageClass
    {
        // Data fields
        private readonly string? connectionString;
        private readonly string? containerName;

        /// <summary>
        /// Master constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="containerName"></param>
        public FileStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        public Stream this[string name]
        {
            get
            {
                // Declare and instantiate a ShareFileClient object.
                ShareFileClient client = new ShareFileClient(connectionString, containerName, name);
                
                return client.DownloadAsync().Result.Value.Content;
            }
            set
            {
                // Declare and instantiate a ShareFileClient object.
                ShareFileClient client = new ShareFileClient(connectionString, containerName, name);

                client.UploadAsync(value);
            }
        }

        /// <summary>
        /// Adds a new file to the container.
        /// </summary>
        /// <param name="name">The name of the file</param>
        /// <param name="bytes">The data (in bytes) of the file</param>
        public async void AddItem(string name, byte[] bytes)
        {
            // Declare and instantiate a ShareFileClient object.
            ShareFileClient client = new ShareFileClient(connectionString, containerName, name);
            // Set the maximum file size to 20 MB.
            client.Create(20000);
            // Upload the file to the container.
            client.UploadAsync(new MemoryStream(bytes));
        }

        /// <summary>
        /// Deletes a file from the container.
        /// </summary>
        /// <param name="name">The name of the file</param>
        public async void DeleteItem(string name)
        {
            // Declare and instantiate a ShareFileClient object.
            ShareFileClient client = new ShareFileClient(connectionString, containerName, name);
            // Delete the file from the container.
            client.DeleteAsync();
        }

        /// <summary>
        /// Deletes all the files in the container.
        /// </summary>
        public async void DeleteAll()
        {
            // Get the root directory client.
            var client = new ShareClient(connectionString, containerName).GetRootDirectoryClient();
            // Get the files and directories from the root path.
            var content = client.GetFilesAndDirectories();

            // Iterate through the collection.
            foreach (var item in content)
            {
                if (item.IsDirectory)
                    // Delete the directory item.
                    client?.DeleteSubdirectoryAsync(item.Name);
                else
                    // Delete the file item.
                    client?.DeleteFileAsync(item.Name);
            }
        }

        /// <summary>
        /// Returns a list of all the file names in the file container.
        /// </summary>
        /// <returns></returns>
        public StringCollection RecieveData()
        {
            // Get the root directory client.
            var client = new ShareClient(connectionString, containerName).GetRootDirectoryClient();
            // Gets the files and directories from the client.
            var content = client.GetFilesAndDirectories();
            // Declare and instantiate a StringCollection object.
            var collection = new StringCollection();

            // Iterate through the content collection.
            foreach(var item in content)
            {
                // Add the item to the collection.
                collection.Add(item.Name);
            }

            return collection;
        }

        /// <summary>
        /// Returns a list of all the file names in a specific directory of the file container.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public StringCollection RecieveData(string path)
        {
            // Get the root directory client.
            var client = new ShareClient(connectionString, containerName).GetRootDirectoryClient();
            // Gets the files and directories from the client.
            var content = client.GetFilesAndDirectories();
            // Declare and instantiate a StringCollection object.
            var collection = new StringCollection();

            // Iterate through the content collection.
            foreach (var item in content)
            {
                if ($"/{item.Name}/".Contains(path))
                    // Add the item to the collection.
                    collection.Add(item.Name);
            }

            return collection;
        }


        /// <summary>
        /// Gets the full path of a file item.
        /// </summary>
        /// <param name="name">The name of the file item</param>
        /// <returns></returns>
        public string GetFullPath(string name)
        {
            // Declare and instantiate a ShareClient object.
            var client = new ShareClient(connectionString, containerName);

            return $"{client.Uri.ToString()}/{name}";
        }
    }
}
