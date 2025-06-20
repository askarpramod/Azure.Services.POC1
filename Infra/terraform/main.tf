provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-servicebus-demo"
  location = "East US"
}

resource "azurerm_servicebus_namespace" "sb" {
  name                = "sb-az-topic-demo"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

resource "azurerm_servicebus_topic" "topic" {
  name                = "orders-topic"
  namespace_name      = azurerm_servicebus_namespace.sb.name
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_servicebus_subscription" "high" {
  name                = "high-priority-orders"
  topic_name          = azurerm_servicebus_topic.topic.name
  namespace_name      = azurerm_servicebus_namespace.sb.name
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_servicebus_subscription_rule" "high_filter" {
  name                = "HighOnly"
  subscription_name   = azurerm_servicebus_subscription.high.name
  topic_name          = azurerm_servicebus_topic.topic.name
  namespace_name      = azurerm_servicebus_namespace.sb.name
  resource_group_name = azurerm_resource_group.rg.name
  filter_type         = "SqlFilter"
  sql_filter          = "Priority = 'High'"
}
