// See https://aka.ms/new-console-template for more information
//Subscriber - Reads messages based on subscription criteria
/*
 //string connStr = "";
//Service BUS Connection string - ASB Namespace => Settings => Shared Access Policies => RootManagedSharedAccessKey => Primary Connection string
 */
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

Console.WriteLine("Read Message from Subscribed Topic!");

//Service BUS Connection string - ASB Namespace => Settings => Shared Access Policies => RootManagedSharedAccessKey => Primary Connection string

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
string connStr = config["ServiceBus:ConnectionString"];
string topicName = "orders-topic";
string subscriptionName = "high-priority-orders";

//Create Client and Processor
var client = new ServiceBusClient(connStr);
ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions()); //sets up a message processor for a specific topic and subscription

/*
This code handles what happens when a message is received
It reads message body, deserializes it to the Order class
Prints order details to the console
*/
processor.ProcessMessageAsync += async args =>
{
    var body = args.Message.Body.ToString();
    var order = JsonSerializer.Deserialize<Order>(body);
    Console.WriteLine($"Received High Priority Order: {order?.Id} - {order?.Item}");
    
    //Acknowledges the message using CompleteMessageAsync so it won’t be processed again
    await args.CompleteMessageAsync(args.Message); //This is done in background : message.IsSettled = true;
};

//Error Handling
processor.ProcessErrorAsync += args =>
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
};

//Start and Dispose
await processor.StartProcessingAsync();
Console.WriteLine("Listening to high-priority-orders subscription...");
Console.ReadKey();
await processor.DisposeAsync();
Console.WriteLine("Reading message processor complete");
record Order(string Id, string Item, string Priority);
