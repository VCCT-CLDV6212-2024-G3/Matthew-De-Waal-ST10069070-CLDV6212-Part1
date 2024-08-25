using Azure.Storage.Queues;

namespace ABCRetail.Models.Queues
{
    public abstract class QueueStorageClass
    {
        // Data fields
        private readonly string? connectionString;
        private readonly string? containerName;

        /// <summary>
        /// Master constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="containerName"></param>
        public QueueStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }

        /// <summary>
        /// Adds a message to the container.
        /// </summary>
        /// <param name="message">The message content to add.</param>
        /// <returns></returns>
        public async Task<string> AddMessage(string message)
        {
            // Declare and instantiate a QueueClient object.
            QueueClient client = new QueueClient(connectionString, containerName);

            return client?.SendMessageAsync(message).Result.Value?.MessageId;
        }

        /// <summary>
        /// Deletes a message from the container by the message id.
        /// </summary>
        /// <param name="id">The id of the message.</param>
        public async void DeleteMessage(string id)
        {
            // Declare and instantiate a QueueClient object.
            QueueClient client = new QueueClient(connectionString, containerName);
            // Recieve a collection of messages from the client.
            var messages = client.ReceiveMessages();

            // Iterate through the message collection
            foreach (var message in messages.Value)
            {
                // Find the message by checking its id.
                if (message.MessageId == id)
                {
                    // Delete the message from the container.
                    client.DeleteMessageAsync(message.MessageId, string.Empty);
                    break;
                }
            }
        }
    }
}
