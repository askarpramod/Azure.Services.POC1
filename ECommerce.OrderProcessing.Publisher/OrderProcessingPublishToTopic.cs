// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
/*
 Use Case: E-commerce Order Processing
Orders are published to a topic (orders-topic)

Subscriptions filter messages:  high-priority-orders for Priority=High | low-priority-orders for others
//string connStr = "";
 */
Console.WriteLine("Send message to ASB Topic!");

//Service BUS Connection string - ASB Namespace => Settings => Shared Access Policies => RootManagedSharedAccessKey => Primary Connection string

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
string connStr = config["ServiceBus:ConnectionString"];

string topicName = "orders-topic";

var client = new ServiceBusClient(connStr);
var sender = client.CreateSender(topicName);

var orders = new[]
{
    new { Id = "O001", Item = "Laptop", Priority = "High" },
    new { Id = "O002", Item = "Mouse", Priority = "Low" },
    new { Id = "O003", Item = "KeyBoard", Priority = "Low" },
    new { Id = "O004", Item = "Mobile1", Priority = "High" },
    new { Id = "O005", Item = "Mobile2", Priority = "Low" },
    new { Id = "O006", Item = "Dummy1", Priority = "Test" },
    new { Id = "O007", Item = "Dummy2", Priority = "Test" },
};

foreach (var order in orders)
{
    var message = new ServiceBusMessage(JsonSerializer.Serialize(order));
    message.ApplicationProperties.Add("Priority", order.Priority);
    await sender.SendMessageAsync(message);
    Console.WriteLine($"Sent order {order.Id}");
}

await sender.DisposeAsync();