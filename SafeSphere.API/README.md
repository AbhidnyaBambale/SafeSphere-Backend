# SafeSphere API

A comprehensive .NET 8 Web API for the SafeSphere mobile application, providing emergency alert management and user authentication features.

## 🚀 Technology Stack

- **.NET 8** - Web API Framework
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Database
- **AutoMapper** - Object mapping
- **Swashbuckle (Swagger)** - API Documentation
- **Serilog** - Structured logging
- **BCrypt.Net** - Password hashing

## 📁 Project Structure

```
SafeSphere.API/
├── Controllers/          # API Controllers
│   ├── UserController.cs
│   └── AlertController.cs
├── Models/              # Entity Models
│   ├── User.cs
│   ├── PanicAlert.cs
│   └── SOSAlert.cs
├── DTOs/                # Data Transfer Objects
│   ├── UserDTOs.cs
│   └── AlertDTOs.cs
├── Data/                # Database Context
│   └── SafeSphereDbContext.cs
├── Repositories/        # Data Access Layer
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── IAlertRepository.cs
│   └── AlertRepository.cs
├── Services/            # Business Logic Layer
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IAlertService.cs
│   └── AlertService.cs
├── Mappings/           # AutoMapper Profiles
│   └── MappingProfile.cs
└── Program.cs          # Application Entry Point
```

## 📋 Prerequisites

Before running the application, ensure you have:

1. **.NET 8 SDK** installed
   ```bash
   dotnet --version  # Should be 8.0 or higher
   ```

2. **PostgreSQL** installed and running
   - Download from: https://www.postgresql.org/download/
   - Default port: 5432

3. **EF Core Tools** installed globally
   ```bash
   dotnet tool install --global dotnet-ef
   ```

## ⚙️ Configuration

### 1. Database Connection

Update the connection string in `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=safesphere_db;Username=postgres;Password=your_password"
  }
}
```

### 2. Create PostgreSQL Database

```sql
-- Connect to PostgreSQL and create database
CREATE DATABASE safesphere_db;

-- Or for development
CREATE DATABASE safesphere_dev_db;
```

## 🔧 Setup Instructions

### Step 1: Restore NuGet Packages

```bash
cd SafeSphere.API
dotnet restore
```

### Step 2: Create Initial Migration

```bash
dotnet ef migrations add InitialCreate
```

### Step 3: Apply Migration to Database

```bash
dotnet ef database update
```

This will create all tables and seed sample data automatically.

### Step 4: Run the Application

```bash
dotnet run
```

The API will start on:
- **HTTPS**: `https://localhost:7000` (or configured port)
- **HTTP**: `http://localhost:5000` (or configured port)

### Step 5: Access Swagger UI

Open your browser and navigate to:
```
https://localhost:7000
```

Swagger UI will load at the root URL in development mode.

## 📝 Database Migrations Commands

### Create a new migration
```bash
dotnet ef migrations add <MigrationName>
```

### Apply migrations to database
```bash
dotnet ef database update
```

### Rollback to a specific migration
```bash
dotnet ef database update <MigrationName>
```

### Remove last migration (if not applied)
```bash
dotnet ef migrations remove
```

### Generate SQL script from migrations
```bash
dotnet ef migrations script
```

### Drop database (BE CAREFUL!)
```bash
dotnet ef database drop
```

## 🎯 API Endpoints

### User Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/user/register` | Register a new user |
| POST | `/api/user/login` | User login |
| GET | `/api/user/{id}` | Get user by ID |
| PUT | `/api/user/{id}` | Update user information |
| DELETE | `/api/user/{id}` | Delete user |

### Panic Alerts

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/alert/panic?userId={id}` | Create panic alert |
| GET | `/api/alert/panic/{id}` | Get panic alert by ID |
| GET | `/api/alert/panic` | Get all panic alerts |
| GET | `/api/alert/panic/user/{userId}` | Get user's panic alerts |
| GET | `/api/alert/panic/active` | Get active panic alerts |
| PATCH | `/api/alert/panic/{id}/status` | Update panic alert status |
| DELETE | `/api/alert/panic/{id}` | Delete panic alert |

### SOS Alerts

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/alert/sos?userId={id}` | Create SOS alert |
| GET | `/api/alert/sos/{id}` | Get SOS alert by ID |
| GET | `/api/alert/sos` | Get all SOS alerts |
| GET | `/api/alert/sos/user/{userId}` | Get user's SOS alerts |
| GET | `/api/alert/sos/unacknowledged` | Get unacknowledged SOS alerts |
| PATCH | `/api/alert/sos/{id}/acknowledge` | Acknowledge SOS alert |
| DELETE | `/api/alert/sos/{id}` | Delete SOS alert |

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | API health status |

## 📊 Sample Data

The database is seeded with sample data on initial migration:

### Users
- **Email**: john.doe@example.com | **Password**: password123
- **Email**: jane.smith@example.com | **Password**: password123

### Sample Alerts
- Pre-populated panic and SOS alerts for testing

## 🔐 Authentication

Currently using **basic authentication** with BCrypt password hashing. In production, consider implementing:
- JWT tokens
- OAuth 2.0
- Identity Server

## 🌐 CORS Configuration

CORS is configured to allow requests from any origin for React Native frontend:

```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```

⚠️ **Note**: In production, restrict CORS to specific origins.

## 📚 Swagger Documentation

Full API documentation is available through Swagger UI when running in development mode. Access it at the root URL:

```
https://localhost:7000
```

Features:
- Interactive API testing
- Request/response schemas
- Example values
- Authentication testing

## 🪵 Logging

Logs are written to:
- **Console** - Real-time logs
- **File** - `logs/safesphere-YYYYMMDD.log` (rolling daily)

## 🧪 Testing the API

### Using Swagger UI
1. Run the application
2. Navigate to `https://localhost:7000`
3. Expand endpoints and click "Try it out"
4. Fill in parameters and execute

### Using cURL

**Register a user:**
```bash
curl -X POST "https://localhost:7000/api/user/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "phone": "+1234567890",
    "password": "password123",
    "emergencyContacts": "Emergency:911"
  }'
```

**Login:**
```bash
curl -X POST "https://localhost:7000/api/user/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123"
  }'
```

**Create Panic Alert:**
```bash
curl -X POST "https://localhost:7000/api/alert/panic?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "additionalInfo": "Emergency situation"
  }'
```

## 🚦 Build and Deploy

### Development Build
```bash
dotnet build
```

### Production Build
```bash
dotnet publish -c Release -o ./publish
```

### Run in Production
```bash
cd publish
dotnet SafeSphere.API.dll
```

## 🐛 Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running: `sudo service postgresql status`
- Check connection string in `appsettings.json`
- Ensure database exists
- Verify username/password credentials

### Migration Errors
- Ensure EF Core tools are installed: `dotnet tool list -g`
- Clear migrations: `dotnet ef migrations remove`
- Drop and recreate database: `dotnet ef database drop`

### Port Already in Use
- Change port in `Properties/launchSettings.json`
- Or set environment variable: `export ASPNETCORE_URLS="https://localhost:7001"`

## 📄 License

This project is part of the SafeSphere application suite.

## 👥 Contact

For questions or support, contact the SafeSphere development team.

---

**Built with ❤️ using .NET 8 and PostgreSQL**

