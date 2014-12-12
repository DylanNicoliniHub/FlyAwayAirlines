using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BusinessDomain.Contracts;
using BusinessDomain.Data;
using BusinessDomain.Implementation;
using Infrastructure;
using Infrastructure.Azure;
using Infrastructure.Azure.Messaging;
using Infrastructure.Messaging;
using Infrastructure.Serialization;
using Microsoft.WindowsAzure;

namespace FlyAwayAIrlines.WebApi
{
    public class Bootstrap
    {
        public static void Run()
        {
            SetAutofacWebAPI();
        }

        private static void SetAutofacWebAPI()
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register other dependencies.
            // New Instance Example
            //builder.Register(c => new FlightPlanService()).As<IFlightPlanService>().InstancePerRequest();
            
            builder.RegisterType<JsonTextSerializer>()
                .As<ITextSerializer>()
                .InstancePerRequest();

            builder.RegisterType<StandardMetadataProvider>()
                .As<IMetadataProvider>()
                .InstancePerRequest();

            // Configure QueueMessageSender
            var connectionString =
                CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var flightPlanQueueName = ConfigurationManager.AppSettings["FlightBookingQueueName"];

            builder.RegisterType<QueueMessageSender>()
                .As<IMessageSender>()
                .WithParameter("connectionString",connectionString)
                .WithParameter("queueName", flightPlanQueueName)
                .InstancePerRequest();

            builder.RegisterType<AzureCommandBus>()
                .As<ICommandBus>()
                .InstancePerRequest();
            
            builder.RegisterType<FlightPlanRepository>()
                .As<IFlightPlanRepository>()
                .InstancePerRequest();

            builder.RegisterType<FlightPlanService>()
                .As<IFlightPlanService>()
                .InstancePerRequest();

            // Build the container.
            var container = builder.Build();

            // Create the depenedency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}