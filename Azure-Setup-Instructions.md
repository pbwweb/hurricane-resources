# Azure Blob Storage Setup Instructions

## Overview

This Hurricane Resources application uses Azure Blob Storage to store and manage icon files for hurricane resources. The application is configured with placeholder connection strings that need to be replaced with your actual Azure Storage Account information.

## Prerequisites

1. An active Azure subscription
2. An Azure Storage Account with Blob Storage enabled

## Azure Storage Account Setup

### 1. Create Azure Storage Account

```bash
# Using Azure CLI
az storage account create \
    --name hurricaneresourcestorage \
    --resource-group your-resource-group \
    --location eastus \
    --sku Standard_LRS \
    --kind StorageV2
```

### 2. Create Blob Container

```bash
# Create the icons container
az storage container create \
    --name icons \
    --account-name hurricaneresourcestorage \
    --public-access blob
```

### 3. Get Connection String

```bash
# Get the connection string for your storage account
az storage account show-connection-string \
    --name hurricaneresourcestorage \
    --resource-group your-resource-group \
    --output table
```

## Configuration

### Update appsettings.json files

Replace the placeholder connection strings in the following files:

1. `HurricaneResources.Web.Admin/appsettings.json`
2. `HurricaneResources.Web.Admin/appsettings.Development.json`
3. `HurricaneResources.Web.User/appsettings.json`
4. `HurricaneResources.Web.User/appsettings.Development.json`

Replace:
```json
"AzureBlobStorage": {
  "ConnectionString": "YOUR_AZURE_STORAGE_CONNECTION_STRING_HERE",
  "ContainerName": "icons"
}
```

With your actual connection string:
```json
"AzureBlobStorage": {
  "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=hurricaneresourcestorage;AccountKey=YOUR_ACCOUNT_KEY;EndpointSuffix=core.windows.net",
  "ContainerName": "icons"
}
```

## Features Implemented

### Admin Application Features
- ✅ **File Upload**: Upload JPG icon files via drag-and-drop or file selection
- ✅ **Automatic File Naming**: Backend generates unique filenames with timestamp and category
- ✅ **Icon Preview**: Display uploaded icons in the admin interface
- ✅ **File Management**: Automatic cleanup of old files when updating resources
- ✅ **Error Handling**: Comprehensive error handling with user feedback

### Blob Storage Service Features
- ✅ **Upload Icons**: Async upload with proper content type and metadata
- ✅ **Delete Icons**: Cleanup old files when resources are updated or deleted
- ✅ **File Validation**: 5MB file size limit and content type validation
- ✅ **URL Generation**: Automatic URL composition for accessing stored icons
- ✅ **Configuration Management**: Centralized Azure storage configuration

### Security & Best Practices
- ✅ **Connection String Security**: Externalized configuration for security
- ✅ **File Validation**: Content type and size validation
- ✅ **Error Handling**: Graceful error handling with logging
- ✅ **Dependency Injection**: Proper service registration and injection
- ✅ **Async Operations**: Non-blocking file operations

## Usage

1. **Adding New Resources**: 
   - Select category and enter resource name and link
   - Upload an icon file (JPG recommended)
   - System automatically generates filename and uploads to Azure

2. **Updating Resources**:
   - Edit resource details
   - Optionally upload a new icon (old icon is automatically deleted)
   - System maintains file consistency

3. **Deleting Resources**:
   - System automatically removes associated icon files from Azure Blob Storage
   - No manual cleanup required

## Testing Without Azure

If you don't have Azure setup yet, the application will run but file uploads will fail gracefully with error messages. The rest of the functionality (viewing resources, managing without file uploads) will work normally.

## Production Considerations

1. **Security**: Store connection strings in Azure Key Vault or environment variables
2. **Performance**: Consider Azure CDN for global icon distribution
3. **Backup**: Enable blob storage backup and versioning
4. **Monitoring**: Set up Azure Monitor for storage metrics and alerts
5. **Scaling**: Consider premium storage tiers for high-traffic scenarios