// See https://aka.ms/new-console-template for more information
/*
 1.
 string? connStr = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
if (string.IsNullOrEmpty(connStr))
{
    Console.WriteLine("Connection string not set.");
    return;
}
 //Service BUS Connection string - ASB Namespace => Settings => Shared Access Policies => RootManagedSharedAccessKey => Primary Connection string
 */
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

Console.WriteLine("Hello, World!");
//Queue Receiver
//string connStr = "";

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
string connStr = config["ServiceBus:ConnectionString"];
string queueName = "que1";

// Create the client
ServiceBusClient client = new ServiceBusClient(connStr);
ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

processor.ProcessMessageAsync += async args =>
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
    await args.CompleteMessageAsync(args.Message);
};

processor.ProcessErrorAsync += args =>
{
    Console.WriteLine($"Error: {args.Exception.Message}");
    return Task.CompletedTask;
};

await processor.StartProcessingAsync();
Console.WriteLine("Waiting for messages...");
Console.ReadKey(); // Wait until you press a key
await processor.StopProcessingAsync();