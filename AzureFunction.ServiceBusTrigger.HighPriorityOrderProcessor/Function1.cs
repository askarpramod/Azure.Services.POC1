using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
//https://www.youtube.com/watch?v=YFKrguSrOAM&list=PL59L9XrzUa-n7jlb-bPuaY4TE-yCYNUnb&index=8 //Rahul Nath
namespace AzureFunction.ServiceBusTrigger.HighPriorityOrderProcessor
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run(
            [ServiceBusTrigger("orders-topic", "high-priority-orders", Connection = "ServiceBusConnString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

             // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
