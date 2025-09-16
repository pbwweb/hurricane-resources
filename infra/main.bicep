targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

// Variables
var tags = { 'azd-env-name': environmentName }

// Create a resource group to contain all resources
resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'rg-${environmentName}'
  location: location
  tags: tags
}

// Deploy resources using a module
module resources 'resources.bicep' = {
  name: 'resources'
  scope: rg
  params: {
    environmentName: environmentName
    location: location
  }
}

// Outputs
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_RESOURCE_GROUP_NAME string = rg.name

// Pass through outputs from resources module
output AZURE_STORAGE_ACCOUNT_NAME string = resources.outputs.AZURE_STORAGE_ACCOUNT_NAME
output AZURE_STORAGE_ACCOUNT_ID string = resources.outputs.AZURE_STORAGE_ACCOUNT_ID
output AZURE_STORAGE_BLOB_ENDPOINT string = resources.outputs.AZURE_STORAGE_BLOB_ENDPOINT

output AZURE_CONTAINER_ENVIRONMENT_NAME string = resources.outputs.AZURE_CONTAINER_ENVIRONMENT_NAME
output AZURE_ADMIN_CONTAINER_APP_NAME string = resources.outputs.AZURE_ADMIN_CONTAINER_APP_NAME
output AZURE_USER_CONTAINER_APP_NAME string = resources.outputs.AZURE_USER_CONTAINER_APP_NAME
output AZURE_ADMIN_CONTAINER_APP_FQDN string = resources.outputs.AZURE_ADMIN_CONTAINER_APP_FQDN
output AZURE_USER_CONTAINER_APP_FQDN string = resources.outputs.AZURE_USER_CONTAINER_APP_FQDN

output SERVICE_ADMIN_URL string = resources.outputs.SERVICE_ADMIN_URL
output SERVICE_USER_URL string = resources.outputs.SERVICE_USER_URL
