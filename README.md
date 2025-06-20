# Azure.Services.POC1
C# Azure Services like Azure Service Bus, Azure Function, Azure Key Vault

# 1. Infrastructure-as-Code - infra deployment
## 1.1 ARM Template (infra/arm/azuredeploy.json): Deploys

Service Bus namespace

Topic (orders-topic)

high-priority-orders and low-priority-orders subscriptions

(with Priority filter support via ARM skirt steps)


## 1.2 Bicep (infra/bicep/main.bicep): Equivalent resource deployment in Bicep


## 1.3 Terraform (infra/terraform/main.tf): Deploys same resources including SQL filters for HighOnly subscription


# 2. Filtering strategy

high-priority-orders: Priority = 'High'

low-priority-orders: Priority = 'Low'

Enhancing telemetry

Application Insights wiring instructions

#3. Key Vault -  Update Program.cs to Load Secret from Key Vault

using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.ServiceBus;

var keyVaultName = "my-keyvault";
var kvUri = $"https://{keyVaultName}.vault.azure.net/";

var config = new ConfigurationBuilder()
    .AddAzureKeyVault(new Uri(kvUri), new DefaultAzureCredential())
    .Build();

string connStr = config["ServiceBusConnectionString"]; // Loaded from Key Vault

var client = new ServiceBusClient(connStr);