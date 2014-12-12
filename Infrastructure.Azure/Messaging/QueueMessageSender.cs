using System;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Infrastructure.Azure.Messaging
{
    public class QueueMessageSender : IMessageSender
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        private QueueClient _client;

        public QueueMessageSender(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(_queueName))
            {
                namespaceManager.CreateQueue(_queueName);
            }

            // Initialize the connection to Service Bus Queue
            _client = QueueClient.CreateFromConnectionString(connectionString, _queueName);
        }

        public void Send(Func<BrokeredMessage> messageFactory)
        {
            _client.Send(messageFactory());
        }

        public void SendAsync(Func<BrokeredMessage> messageFactory)
        {
            throw new NotImplementedException();
        }

        public void SendAsync(Func<BrokeredMessage> messageFactory, Action successCallback, Action<Exception> exceptionCallback)
        {
            throw new NotImplementedException();
        }

        public event EventHandler Retrying;
    }
}
