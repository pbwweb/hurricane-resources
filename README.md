# Hurricane Resources App

A mobile-friendly web application designed to help users access critical hurricane emergency resources in their area. Built with .NET 9.0, Blazor Server, and .NET Aspire for cloud-native development.

## 🌀 Overview

The Hurricane Resources App provides a centralized platform for accessing emergency resources during hurricane events. It consists of two main applications:

- **User App**: Mobile-optimized interface for accessing emergency resources
- **Admin App**: Administrative interface for managing resource database

## 🏗️ Architecture

### Project Structure

```
HurricaneResources/
├── HurricaneResources.AppHost/           # .NET Aspire AppHost orchestration
├── HurricaneResources.ServiceDefaults/   # Shared service configurations
├── HurricaneResources.Shared/           # Shared libraries and models
├── HurricaneResources.Web.Admin/       # Admin web application
├── HurricaneResources.Web.User/        # User-facing web application
└── assets/                             # Static assets and database
```

### Technology Stack

- **.NET 9.0**: Latest .NET framework
- **Blazor Server**: Server-side rendering with real-time UI updates
- **.NET Aspire 9.4.2**: Cloud-native application platform for distributed apps
- **Entity Framework Core 9.0.9**: Object-relational mapping
- **SQLite**: Lightweight database for local storage
- **Azure Blob Storage**: Cloud storage for resource icons
- **Bootstrap 5**: Responsive UI framework

## 📱 Applications

### User Application (Mobile-Optimized)

**Features:**
- Mobile-first responsive design
- Touch-friendly resource grid interface
- Real-time resource loading
- Fallback icon handling
- External link navigation to resources

**Key Components:**
- **Home.razor**: Main resource display page with responsive grid
- **Mobile-optimized CSS**: Touch targets and responsive design
- **Error handling**: Graceful fallback for missing icons

**Technical Details:**
- Read-only database access (no migrations)
- Optimized for mobile browsers
- Server-side rendering for fast load times
- External link handling for resource access

### Admin Application

**Features:**
- Resource management (CRUD operations)
- File upload for custom icons
- Azure Blob Storage integration
- Database schema management
- Resource categorization

**Key Components:**
- **Resource Management**: Add, edit, delete resources
- **File Upload**: Custom icon management
- **Azure Integration**: Blob storage for scalable file storage
- **Database Migrations**: Handles schema creation and updates

## 🗃️ Data Models

### HurricaneResource
```csharp
public class HurricaneResource
{
    public int Id { get; set; }
    public ResourceCategory Category { get; set; }
    public string Name { get; set; }
    public string IconUrl { get; set; }
    public string ResourceLink { get; set; }
    public string FileName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### ResourceCategory Enum
- Emergency Services
- Shelters  
- Medical Facilities
- Food Banks
- Supply Distribution
- Transportation
- Communication
- Relief Centers

## 🔧 Configuration

### Database Configuration

The application uses a shared SQLite database stored in the `assets` folder:

```csharp
// DatabasePathHelper.cs - Centralized path management
public static class DatabasePathHelper
{
    public static string GetDatabasePath() => 
        Path.Combine(GetAssetsDirectory(), "hurricane_resources.db");
    
    public static string GetConnectionString() => 
        $"Data Source={GetDatabasePath()}";
}
```

### Azure Blob Storage

Configuration for cloud-based icon storage:

```json
{
  "AzureBlobStorage": {
    "ConnectionString": "your-connection-string",
    "ContainerName": "icons",
    "BaseUrl": "https://youraccount.blob.core.windows.net",
    "DefaultIconUrl": "https://pbwblobs.blob.core.windows.net/icons/emergency-20250916003531.jpg"
  }
}
```

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Azure Storage Account (optional, for cloud storage)

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd mobile-framer
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run with .NET Aspire**
   ```bash
   dotnet run --project HurricaneResources.AppHost
   ```

4. **Access applications**
   - User App: Usually available at `https://localhost:5001`
   - Admin App: Usually available at `https://localhost:5002`
   - Aspire Dashboard: Usually available at `https://localhost:15888`

### First-Time Setup

The application includes seeded data with 15 pre-configured emergency resources:

- **Emergency Services**: Broward County Emergency, Florida Emergency Management, EOC
- **Shelters**: Hurricane Shelters, Red Cross, Special Needs
- **Medical**: Broward Health, Memorial Healthcare
- **Food**: Feeding South Florida
- **Supplies**: Salvation Army, Emergency Supply Kits
- **Transportation**: Evacuation Routes, BCT Emergency Transport
- **Additional**: FEMA Assistance, National Weather Service

## 🏃‍♂️ Running the Application

### Development Mode

The project uses .NET Aspire for orchestration:

```bash
# Start both applications with Aspire orchestration
dotnet watch run --project HurricaneResources.AppHost
```

### Individual Applications

```bash
# User application only
dotnet run --project HurricaneResources.Web.User

# Admin application only  
dotnet run --project HurricaneResources.Web.Admin
```

## 🔒 Security & Data Management

### Database Security
- **Admin App**: Full CRUD operations and schema management
- **User App**: Read-only access, no database migrations
- **Shared Database**: Single source of truth in assets folder

### File Storage
- Local development: Files stored in project assets
- Production: Azure Blob Storage integration
- Fallback handling: Default icons for missing files

## 🎨 UI/UX Features

### Mobile-First Design
- Responsive grid layout optimized for touch interfaces
- Large touch targets (minimum 44px)
- Readable typography on small screens
- Fast loading with server-side rendering

### Accessibility
- Semantic HTML structure
- Alt text for all images
- High contrast color schemes
- Keyboard navigation support

### Progressive Enhancement
- Graceful fallback for failed image loads
- Loading states for async operations
- Error handling with user-friendly messages

## 📊 Database Schema

### Resource Categories
The system supports 8 main resource categories with extensible design:

```sql
-- Example seed data structure
INSERT INTO Resources (Id, Category, Name, IconUrl, ResourceLink, FileName, IsActive) 
VALUES 
(1, 'Emergency', 'Broward County Emergency Management', 'default-icon-url', 'resource-link', 'filename.jpg', 1);
```

### Indexing Strategy
- Primary key on `Id`
- Indexes on `Category`, `IsActive`, and `FileName`
- Optimized for read-heavy workloads

## 🔄 Development Workflow

### Code Organization
- **Shared Project**: Common models, services, and configurations
- **Service Defaults**: Aspire service configurations and telemetry
- **Separation of Concerns**: Clear boundaries between admin and user functionality

### Database Migrations
- Admin app handles all database schema changes
- User app operates in read-only mode
- Centralized database path management

### Asset Management
- Static assets stored in dedicated assets folder
- Gitignore configured for database files
- Azure Blob Storage integration for scalable file storage

## 🚀 Deployment

### Prerequisites for Production
- .NET 9.0 Runtime
- Azure Storage Account (recommended)
- SSL Certificate for HTTPS

### Environment Configuration
- Configure Azure Blob Storage connection strings
- Set up proper CORS policies for file uploads
- Configure logging and monitoring

### Aspire Deployment
The application is designed for cloud-native deployment with .NET Aspire:
- Container orchestration ready
- Health checks and telemetry built-in
- Service discovery and resilience patterns

## 🛠️ Troubleshooting

### Common Issues

**404 Icon Errors**
- Check Azure Blob Storage configuration
- Verify default icon URL accessibility
- Ensure fallback handling is working

**Database Connection Issues**
- Verify database path in assets folder
- Check file permissions
- Ensure Admin app runs first for schema creation

**Build Errors**
- Restore NuGet packages: `dotnet restore`
- Clear bin/obj folders: `dotnet clean`
- Rebuild solution: `dotnet build`

## 🤝 Contributing

### Development Setup
1. Fork the repository
2. Create feature branch
3. Follow C# coding conventions
4. Add unit tests for new features
5. Submit pull request

### Code Style
- Use C# 12 features and nullable reference types
- Follow .NET naming conventions
- Add XML documentation for public APIs
- Maintain responsive design principles

## 📝 License

This project is designed for emergency resource management and community support during hurricane events.

## 🆘 Emergency Resources

This application provides access to critical emergency resources including:

- **Emergency Management**: County and state emergency operations
- **Shelter Information**: Public and special needs shelters
- **Medical Services**: Emergency healthcare facilities
- **Food Distribution**: Food banks and emergency feeding
- **Supply Distribution**: Emergency supplies and relief items
- **Transportation**: Evacuation routes and emergency transport
- **Communication**: Emergency contact information
- **Relief Services**: Disaster relief organizations

For immediate emergencies, always dial **911**.

---

Built with ❤️ for community safety and hurricane preparedness.