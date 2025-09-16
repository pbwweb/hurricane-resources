@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

// Variables
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = { 'azd-env-name': environmentName }

// Log Analytics Workspace for Container Apps
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'log-${resourceToken}'
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// Container Apps Environment
resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: 'cae-${resourceToken}'
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
}

// Storage Account for Hurricane Resources
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: 'st${resourceToken}'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  tags: tags
  properties: {
    allowBlobPublicAccess: true
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
  }
}

// Blob Services for Storage Account
resource blobServices 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
  parent: storageAccount
  name: 'default'
  properties: {}
}

// Icons Container
resource iconsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobServices
  name: 'icons'
  properties: {
    publicAccess: 'Blob'
  }
}

// Container App for Admin Application
resource adminContainerApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'ca-admin-${resourceToken}'
  location: location
  tags: tags
  properties: {
    managedEnvironmentId: containerAppsEnvironment.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        allowInsecure: false
        traffic: [
          {
            weight: 100
            latestRevision: true
          }
        ]
      }
    }
    template: {
      containers: [
        {
          name: 'hurricane-admin-app'
          image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'AzureBlobStorage__AccountName'
              value: storageAccount.name
            }
            {
              name: 'AzureBlobStorage__ContainerName'
              value: 'icons'
            }
            {
              name: 'AzureBlobStorage__BaseUrl'
              value: storageAccount.properties.primaryEndpoints.blob
            }
            {
              name: 'AzureBlobStorage__DefaultIconUrl'
              value: '${storageAccount.properties.primaryEndpoints.blob}icons/emergency-20250916003531.jpg'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 5
      }
    }
  }
}

// Container App for User Application
resource userContainerApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: 'ca-user-${resourceToken}'
  location: location
  tags: tags
  properties: {
    managedEnvironmentId: containerAppsEnvironment.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        allowInsecure: false
        traffic: [
          {
            weight: 100
            latestRevision: true
          }
        ]
      }
    }
    template: {
      containers: [
        {
          name: 'hurricane-user-app'
          image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'AzureBlobStorage__AccountName'
              value: storageAccount.name
            }
            {
              name: 'AzureBlobStorage__ContainerName'
              value: 'icons'
            }
            {
              name: 'AzureBlobStorage__BaseUrl'
              value: storageAccount.properties.primaryEndpoints.blob
            }
            {
              name: 'AzureBlobStorage__DefaultIconUrl'
              value: '${storageAccount.properties.primaryEndpoints.blob}icons/emergency-20250916003531.jpg'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 10
      }
    }
  }
}

// Outputs
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId

// Storage Account outputs
output AZURE_STORAGE_ACCOUNT_NAME string = storageAccount.name
output AZURE_STORAGE_ACCOUNT_ID string = storageAccount.id
output AZURE_STORAGE_BLOB_ENDPOINT string = storageAccount.properties.primaryEndpoints.blob

// Container Apps outputs
output AZURE_CONTAINER_ENVIRONMENT_NAME string = containerAppsEnvironment.name
output AZURE_ADMIN_CONTAINER_APP_NAME string = adminContainerApp.name
output AZURE_USER_CONTAINER_APP_NAME string = userContainerApp.name
output AZURE_ADMIN_CONTAINER_APP_FQDN string = adminContainerApp.properties.configuration.ingress.fqdn
output AZURE_USER_CONTAINER_APP_FQDN string = userContainerApp.properties.configuration.ingress.fqdn

// Service URLs
output SERVICE_ADMIN_URL string = 'https://${adminContainerApp.properties.configuration.ingress.fqdn}'
output SERVICE_USER_URL string = 'https://${userContainerApp.properties.configuration.ingress.fqdn}'
