# Azure.Services.POC1
C# Azure Services like Service Bus, Function App

# 1. Infrastructure-as-Code - nfra deployment
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
