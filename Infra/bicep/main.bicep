param namespaceName string

resource sb 'Microsoft.ServiceBus/namespaces@2023-01-01-preview' = {
  name: namespaceName
  location: resourceGroup().location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
}

resource topic 'Microsoft.ServiceBus/namespaces/topics@2023-01-01-preview' = {
  name: '${sb.name}/orders-topic'
}

resource highSub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2023-01-01-preview' = {
  name: '${sb.name}/orders-topic/high-priority-orders'
}

resource lowSub 'Microsoft.ServiceBus/namespaces/topics/subscriptions@2023-01-01-preview' = {
  name: '${sb.name}/orders-topic/low-priority-orders'
}
