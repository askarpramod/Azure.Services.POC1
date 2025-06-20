// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus; //install this package - Azure.Messaging.ServiceBus
using Microsoft.Extensions.Configuration;

//Queue Sender
Console.WriteLine("Queue sender !");

//Service BUS Connection string - ASB Namespace => Settings => Shared Access Policies => RootManagedSharedAccessKey => Primary Connection string
//string connStr = ""; 
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
string connStr = config["ServiceBus:ConnectionString"];
string queueName = "que1";

// Create the client
ServiceBusClient client = new ServiceBusClient(connStr);
ServiceBusSender sender = client.CreateSender(queueName);

// Create a message
ServiceBusMessage message = new ServiceBusMessage($"Hello from .NET POC! added at :{DateTime.Now}");

// Send it
await sender.SendMessageAsync(message);
Console.WriteLine("Message sent.");

