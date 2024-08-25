using Azure.Storage.Queues;

namespace ABCRetail.Models.Queues
{
    public abstract class QueueStorageClass
    {
        private readonly string? connectionString;
        private readonly string? containerName;

        public QueueStorageClass(string connectionString, string containerName)
        {
            this.connectionString = connectionString;
            this.containerName = containerName;
        }


        public async Task<string> AddMessage(string message)
        {
            QueueClient client = new QueueClient(connectionString, containerName);
            return client?.SendMessageAsync(message).Result.Value?.MessageId;
        }

        public async void DeleteMessage(string id)
        {
            QueueClient client = new QueueClient(connectionString, containerName);
            var messages = client.ReceiveMessages();

            foreach (var message in messages.Value)
            {
                if (message.MessageId == id)
                {
                    client.DeleteMessageAsync(message.MessageId, string.Empty);
                    break;
                }
            }
        }
    }
}
