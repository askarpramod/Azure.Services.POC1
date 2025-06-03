// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus; //install this package - Azure.Messaging.ServiceBus

//Queue Sender
Console.WriteLine("Queue sender !");


string connectionString = "Endpoint=sb://azb-nsp1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NgEfuJIqPW4N4P85XakDF+tH7S7ppQWNz+ASbP/S27o=";
string queueName = "que1";

// Create the client
ServiceBusClient client = new ServiceBusClient(connectionString);
ServiceBusSender sender = client.CreateSender(queueName);

// Create a message
ServiceBusMessage message = new ServiceBusMessage($"Hello from .NET POC! added at :{DateTime.Now}");

// Send it
await sender.SendMessageAsync(message);
Console.WriteLine("Message sent.");

