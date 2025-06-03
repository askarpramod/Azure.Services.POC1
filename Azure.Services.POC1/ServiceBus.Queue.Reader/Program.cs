// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

Console.WriteLine("Hello, World!");
//Queue Receiver
string connectionString = "Endpoint=sb://azb-nsp1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NgEfuJIqPW4N4P85XakDF+tH7S7ppQWNz+ASbP/S27o=";
string queueName = "que1";

// Create the client
ServiceBusClient client = new ServiceBusClient(connectionString);
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