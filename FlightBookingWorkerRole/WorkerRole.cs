using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using BusinessDomain.Data;
using BusinessDomain.Implementation.Commands;
using BusinessDomain.Implementation.Handlers;
using Infrastructure.Messaging;
using Infrastructure.Serialization;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace FlightBookingWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        // The name of your queue
        private const string QueueName = "flightbookingqueue";
        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        private QueueClient Client;
        private readonly ManualResetEvent CompletedEvent = new ManualResetEvent(false);
        private readonly Dictionary<Type, ICommandHandler> handlers = new Dictionary<Type, ICommandHandler>();

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            Client.OnMessage(receivedMessage =>
            {
                try
                {
                    // Process the message
                    Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());

                    ITextSerializer serializer = new JsonTextSerializer();
                    using (var stream = receivedMessage.GetBody<Stream>())
                    using (var reader = new StreamReader(stream))
                    {
                        var payload = serializer.Deserialize(reader);

                        var commandType = payload.GetType();
                        ICommandHandler handler = null;

                        if (handlers.TryGetValue(commandType, out handler))
                        {
                            // Invoke message handler to handle the message
                            ((dynamic) handler).Handle((dynamic) payload);


                            // Finish recieving the message and dequeue
                            receivedMessage.Complete();
                        }
                        else
                        {
                            receivedMessage.DeadLetter();
                        }
                    }
                }
                catch
                {
                    // Handle any message processing specific exceptions here
                    receivedMessage.DeadLetter();
                }
            });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            const string databaseConnectionString = @"data source=tcp:qpyirjq1xw.database.windows.net,1433;initial catalog=FlyAwayAirlinesWriteOnly;persist security info=True;user id=DylanNicolini@qpyirjq1xw;password=Test1234#;MultipleActiveResultSets=True;App=EntityFramework";
            var scsb = new SqlConnectionStringBuilder(databaseConnectionString);

            // Add the FlightBookingEvent Handler to the BookFlight Command
            handlers.Add(typeof (BookFlight), new FlightBookingHandler(new FlightPlanRepository(scsb.ConnectionString)));

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }
    }
}