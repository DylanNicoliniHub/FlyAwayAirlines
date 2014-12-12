using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Infrastructure.Azure.Messaging
{
    public class QueueMessageReciever : IMessageReceiver
    {
        private static readonly TimeSpan ReceiveLongPollingTimeout = TimeSpan.FromMinutes(1);
        private CancellationTokenSource cancellationSource;
        private QueueClient _client;
        private string subscription;
        private readonly string _connectionString;
        private readonly bool _processInParallel;
        private readonly string _queueName;
        private readonly object lockObject = new object();
        private readonly bool processInParallel;
        private readonly Uri serviceUri;
        private readonly TokenProvider tokenProvider;
        private readonly string topic;


        public QueueMessageReciever(string connectionString, string queueName, bool processInParallel = false)
        {
            _connectionString = connectionString;
            _queueName = queueName;
            _processInParallel = processInParallel;

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

        /// <summary>
        ///     Handler for incoming messages. The return value indicates whether the message should be disposed.
        /// </summary>
        protected Func<BrokeredMessage, MessageReleaseAction> MessageHandler { get; private set; }

        public void Start(Func<BrokeredMessage, MessageReleaseAction> messageHandler)
        {
            lock (lockObject)
            {
                MessageHandler = messageHandler;
                cancellationSource = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                    this.ReceiveMessages(cancellationSource.Token),
                    cancellationSource.Token);
            }
        }

        public void Stop()
        {
          
        }

        protected virtual MessageReleaseAction InvokeMessageHandler(BrokeredMessage message)
        {
            return this.MessageHandler != null ? this.MessageHandler(message) : MessageReleaseAction.AbandonMessage;
        }

        private void ReceiveMessages(CancellationToken cancellationToken)
        {
            // Declare an action to receive the next message in the queue or end if cancelled.
            Action receiveNext = null;

           

        }

        private void ReleaseMessage(BrokeredMessage msg, MessageReleaseAction releaseAction, long processingElapsedMilliseconds, long schedulingElapsedMilliseconds, Stopwatch roundtripStopwatch)
        {
            switch (releaseAction.Kind)
            {
                case MessageReleaseActionKind.Complete:
                    msg.CompleteAsync();
                    break;
                case MessageReleaseActionKind.Abandon:
                    msg.AbandonAsync();
                    break;
                case MessageReleaseActionKind.DeadLetter:
                    msg.DeadLetterAsync();
                    break;
                default:
                    break;
            }
        }

    }
}