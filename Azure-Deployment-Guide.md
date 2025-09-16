# Azure Deployment Instructions

This document provides step-by-step instructions for deploying the Hurricane Resources application to Azure using Azure Developer CLI (azd).

## Prerequisites

1. **Azure Developer CLI (azd)** - Install from: https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd
2. **Docker Desktop** - Required for containerization
3. **Azure CLI (az)** - Install from: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
4. **.NET 9 SDK** - Install from: https://dotnet.microsoft.com/download

## Architecture Overview

The deployment creates the following Azure resources:

- **Resource Group**: Contains all Hurricane Resources infrastructure
- **Container Apps Environment**: Hosts the containerized applications
- **Log Analytics Workspace**: Provides logging and monitoring
- **Storage Account**: Stores hurricane resource icons with public blob access
- **Container Apps**: 
  - Admin App: For managing hurricane resources
  - User App: Public-facing application for users

## Deployment Steps

### 1. Initialize Azure Developer CLI

```bash
# Navigate to project root
cd /path/to/hurricane-resources

# Initialize AZD (if not already done)
azd init

# When prompted, select "Use code in the current directory"
# Environment name: hurricane-resources-dev (or your preference)
```

### 2. Login to Azure

```bash
# Login to Azure
azd auth login

# Verify login
az account show
```

### 3. Deploy Infrastructure and Applications

```bash
# Deploy everything (infrastructure + applications)
azd up

# Follow prompts:
# - Select Azure subscription
# - Choose deployment location (e.g., eastus2, westus2)
# - Confirm deployment
```

The deployment process will:
1. Build Docker images for Admin and User applications
2. Deploy Bicep infrastructure templates
3. Push container images to Azure Container Registry
4. Deploy applications to Azure Container Apps
5. Configure environment variables and networking

### 4. Post-Deployment Configuration

After successful deployment, you'll receive output with:
- Admin App URL: `https://ca-admin-[unique-id].region.azurecontainerapps.io`
- User App URL: `https://ca-user-[unique-id].region.azurecontainerapps.io`
- Storage Account details for icon management

### 5. Upload Initial Icons (Optional)

Use the Admin application to upload hurricane resource icons:

1. Navigate to the Admin App URL
2. Go to Resources management
3. Upload icons for different resource categories
4. Icons will be stored in Azure Blob Storage with public access

## Environment Variables

The applications use the following environment variables (automatically configured):

- `ASPNETCORE_ENVIRONMENT=Production`
- `AzureBlobStorage__AccountName`: Storage account name
- `AzureBlobStorage__ContainerName`: "icons"
- `AzureBlobStorage__BaseUrl`: Storage account blob endpoint
- `AzureBlobStorage__DefaultIconUrl`: Default emergency icon URL

## Monitoring and Management

### View Application Logs

```bash
# View deployment logs
azd monitor --live

# View specific service logs
az containerapp logs show --name ca-admin-[unique-id] --resource-group rg-hurricane-resources-dev
```

### Scale Applications

```bash
# Scale User app for higher traffic
az containerapp update --name ca-user-[unique-id] --resource-group rg-hurricane-resources-dev --max-replicas 20
```

### Update Applications

```bash
# Redeploy after code changes
azd deploy

# Or deploy specific service
azd deploy admin
azd deploy user
```

## Cost Optimization

- Container Apps scale to zero when not in use
- Storage account uses Standard LRS for cost efficiency
- Log Analytics workspace has 30-day retention

## Security Features

- HTTPS-only communication
- Azure Container Apps managed certificates
- Azure Blob Storage with secure configuration
- Managed identity support for enhanced security

## Troubleshooting

### Common Issues

1. **Docker Build Errors**: Ensure Docker Desktop is running
2. **Azure Access Issues**: Verify Azure CLI login and subscription access
3. **Resource Naming Conflicts**: Use unique environment names
4. **Container Startup Issues**: Check application logs in Azure Portal

### Useful Commands

```bash
# View current environment
azd env list

# View environment variables
azd env get-values

# Clean up resources
azd down

# Force rebuild
azd deploy --force-rebuild
```

## Development Workflow

For ongoing development:

1. Make code changes locally
2. Test with `dotnet run` or Docker locally
3. Deploy updates with `azd deploy`
4. Monitor application health in Azure Portal

## Support

For issues related to:
- **Azure Developer CLI**: https://github.com/Azure/azure-dev
- **Azure Container Apps**: https://docs.microsoft.com/en-us/azure/container-apps/
- **Azure Blob Storage**: https://docs.microsoft.com/en-us/azure/storage/blobs/

---

**Note**: The first deployment may take 10-15 minutes as Azure provisions all resources and builds container images. Subsequent deployments will be faster as infrastructure is already in place.