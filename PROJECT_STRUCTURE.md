# SafeSphere Backend - Project Structure

## 📁 Complete Folder Structure

```
SafeSphere.API/
│
├── Controllers/                    # API Controllers
│   ├── UserController.cs          # User registration, login, CRUD operations
│   └── AlertController.cs         # Panic and SOS alerts management
│
├── Models/                        # Entity Models (Database Tables)
│   ├── User.cs                   # User entity with authentication
│   ├── PanicAlert.cs             # Panic alert entity with location tracking
│   └── SOSAlert.cs               # SOS alert entity with acknowledgment
│
├── DTOs/                         # Data Transfer Objects
│   ├── UserDTOs.cs              # RegisterUserDto, LoginUserDto, UserResponseDto, UpdateUserDto
│   └── AlertDTOs.cs             # CreatePanicAlertDto, PanicAlertResponseDto, CreateSOSAlertDto, etc.
│
├── Data/                         # Database Context
│   └── SafeSphereDbContext.cs   # EF Core DbContext with entity configurations
│
├── Repositories/                 # Data Access Layer (Repository Pattern)
│   ├── IUserRepository.cs       # User repository interface
│   ├── UserRepository.cs        # User repository implementation
│   ├── IAlertRepository.cs      # Alert repository interface
│   └── AlertRepository.cs       # Alert repository implementation
│
├── Services/                     # Business Logic Layer (Service Pattern)
│   ├── IUserService.cs          # User service interface
│   ├── UserService.cs           # User service with business logic
│   ├── IAlertService.cs         # Alert service interface
│   └── AlertService.cs          # Alert service with business logic
│
├── Mappings/                     # AutoMapper Configuration
│   └── MappingProfile.cs        # Object-to-object mapping profiles
│
├── Properties/                   # Project Properties
│   └── launchSettings.json      # Development launch configuration
│
├── logs/                         # Log Files (auto-generated)
│   └── safesphere-*.log         # Daily rolling log files
│
├── appsettings.json             # Production configuration
├── appsettings.Development.json # Development configuration
├── Program.cs                   # Application entry point and configuration
├── SafeSphere.API.csproj        # Project file with NuGet packages
├── README.md                    # Complete API documentation
└── .gitignore                   # Git ignore rules
```

## 🎯 Architecture Pattern

The project follows a **Layered Architecture** pattern:

```
┌─────────────────────────────────────┐
│         Controllers Layer           │  ← API Endpoints (HTTP)
├─────────────────────────────────────┤
│          Services Layer             │  ← Business Logic
├─────────────────────────────────────┤
│        Repositories Layer           │  ← Data Access
├─────────────────────────────────────┤
│     Entity Framework Core (ORM)     │  ← Database Abstraction
├─────────────────────────────────────┤
│          PostgreSQL Database        │  ← Data Storage
└─────────────────────────────────────┘
```

## 📦 NuGet Packages

### Core Packages
- `Microsoft.EntityFrameworkCore` (8.0.11)
- `Microsoft.EntityFrameworkCore.Design` (8.0.11)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (8.0.11)

### Mapping & Documentation
- `AutoMapper` (13.0.1)
- `AutoMapper.Extensions.Microsoft.DependencyInjection` (13.0.1)
- `Swashbuckle.AspNetCore` (6.6.2)

### Logging
- `Serilog.AspNetCore` (8.0.3)
- `Serilog.Sinks.Console` (6.0.0)
- `Serilog.Sinks.File` (6.0.0)

### Security
- `BCrypt.Net-Next` (4.0.3)

## 🗄️ Database Schema

### Users Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PRIMARY KEY, AUTO INCREMENT |
| Name | VARCHAR(100) | NOT NULL |
| Email | VARCHAR(255) | NOT NULL, UNIQUE |
| Phone | VARCHAR(20) | NOT NULL |
| PasswordHash | TEXT | NOT NULL |
| EmergencyContacts | VARCHAR(1000) | NULL |
| CreatedAt | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP |
| UpdatedAt | TIMESTAMP | NULL |

### PanicAlerts Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PRIMARY KEY, AUTO INCREMENT |
| UserId | INT | FOREIGN KEY → Users(Id) |
| LocationLat | DOUBLE | NOT NULL |
| LocationLng | DOUBLE | NOT NULL |
| Timestamp | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP |
| Status | VARCHAR(50) | NOT NULL, DEFAULT 'Active' |
| AdditionalInfo | VARCHAR(500) | NULL |

### SOSAlerts Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PRIMARY KEY, AUTO INCREMENT |
| UserId | INT | FOREIGN KEY → Users(Id) |
| Message | VARCHAR(500) | NOT NULL |
| Location | VARCHAR(255) | NOT NULL |
| LocationLat | DOUBLE | NULL |
| LocationLng | DOUBLE | NULL |
| Timestamp | TIMESTAMP | DEFAULT CURRENT_TIMESTAMP |
| Acknowledged | BOOLEAN | DEFAULT FALSE |
| AcknowledgedAt | TIMESTAMP | NULL |

## 🔧 Configuration Files

### appsettings.json
- Connection strings
- Logging configuration
- Serilog settings
- Production environment settings

### appsettings.Development.json
- Development database connection
- Enhanced logging for debugging
- Development-specific settings

### launchSettings.json
- Development server URLs
- Environment variables
- Launch profiles

## 🚀 Key Features

1. **User Authentication**
   - Registration with BCrypt password hashing
   - Login with email/password
   - User profile management

2. **Panic Alert System**
   - Create panic alerts with GPS coordinates
   - Track alert status (Active, Resolved, Cancelled)
   - Filter by user or status

3. **SOS Alert System**
   - Send SOS messages with location
   - Acknowledgment system
   - Filter unacknowledged alerts

4. **Cross-Origin Resource Sharing (CORS)**
   - Configured for React Native frontend
   - Allows all origins in development

5. **API Documentation**
   - Interactive Swagger UI
   - Complete endpoint documentation
   - Request/response examples

6. **Logging**
   - Structured logging with Serilog
   - Console and file output
   - Daily rolling log files

## 🔐 Security Considerations

### Current Implementation
- BCrypt password hashing (cost factor: default)
- Input validation with Data Annotations
- Entity Framework SQL injection protection

### Future Enhancements
- JWT token-based authentication
- Role-based authorization
- Rate limiting
- API key authentication
- HTTPS enforcement in production
- CORS origin restrictions

## 📊 Design Patterns Used

1. **Repository Pattern** - Data access abstraction
2. **Service Pattern** - Business logic separation
3. **Dependency Injection** - Loose coupling
4. **DTO Pattern** - Data transfer and validation
5. **Factory Pattern** - DbContext creation

## 🧪 Testing Strategy (Future)

### Unit Tests
- Service layer testing
- Repository layer testing
- Model validation testing

### Integration Tests
- API endpoint testing
- Database operations testing
- Authentication flow testing

### Tools to Consider
- xUnit
- Moq
- FluentAssertions
- TestContainers (PostgreSQL)

## 📈 Scalability Considerations

### Current Implementation
- Async/await for all database operations
- Dependency injection for testability
- Separation of concerns

### Future Improvements
- Caching (Redis)
- Message queuing (RabbitMQ)
- Load balancing
- Database read replicas
- API versioning
- Health checks and monitoring

## 📝 Development Workflow

1. **Database Changes**
   ```bash
   dotnet ef migrations add <MigrationName>
   dotnet ef database update
   ```

2. **Add New Entity**
   - Create model in `Models/`
   - Add DbSet to `SafeSphereDbContext`
   - Create repository interface and implementation
   - Create service interface and implementation
   - Create controller
   - Create DTOs
   - Add AutoMapper mapping

3. **Run Application**
   ```bash
   dotnet run
   ```

4. **Build for Production**
   ```bash
   dotnet publish -c Release
   ```

## 🔗 Related Projects

- **SafeSphere Mobile App** - React Native frontend
- **SafeSphere Admin Panel** - Management dashboard (future)
- **SafeSphere WebSocket Service** - Real-time notifications (future)

---

**Last Updated**: October 2025

