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


# 4. appsettings.json cachesettings.json
var builder = WebApplication.CreateBuilder(args);
    var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("cachesettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"cachesettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


# 5. Rate limiter

#region Rate Limter
    var ratelimitInterval = Convert.ToInt32(configuration["RateLimiter:TimeInterval"]);
    var ratelimitMaxRequestAllowed = Convert.ToInt32(configuration["RateLimiter:MaxRequestAllowed"]);
    builder.Services.AddRateLimiter(options =>
    {
        options.OnRejected = async (context, token) =>
        {
            context.HttpContext.Response.StatusCode = 429;
            await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
        };
        options.AddPolicy(ratelimiterpolicy, context => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: context.Request.HttpContext.Connection?.RemoteIpAddress?.ToString(),
        factory: partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = ratelimitMaxRequestAllowed,
            QueueLimit = 0,
            Window = TimeSpan.FromSeconds(ratelimitInterval)
        }));

    });

    #endregion


# 6. Multiple implementation of the service
#region Caching Setup
    builder.Services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

    if (enableDapr)
    {
        builder.Services.AddDaprClient();
        builder.Services.AddScoped<ICacheService, CacheServiceDapr>();
    }
    else
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICacheService, CacheServiceMemory>();
    }
    #endregion
