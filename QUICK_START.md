# SafeSphere API - Quick Start Guide

## ⚡ Get Started in 5 Minutes

### Prerequisites Checklist
- ✅ .NET 8 SDK installed
- ✅ PostgreSQL installed and running
- ✅ EF Core Tools installed globally

### Step 1: Install EF Core Tools (if not installed)
```bash
dotnet tool install --global dotnet-ef
```

### Step 2: Configure Database
Edit `SafeSphere.API/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=safesphere_dev_db;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  }
}
```

### Step 3: Create Database
```sql
-- Connect to PostgreSQL (psql, pgAdmin, or any client)
CREATE DATABASE safesphere_dev_db;
```

### Step 4: Navigate to Project
```bash
cd SafeSphere.API
```

### Step 5: Apply Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 6: Run the Application
```bash
dotnet run
```

### Step 7: Open Swagger UI
Open your browser and navigate to:
```
https://localhost:7000
```
Or check the console output for the actual URL.

## 🎉 You're Ready!

The API is now running with:
- ✅ Database created and seeded with sample data
- ✅ 2 sample users (password: "password123")
  - john.doe@example.com
  - jane.smith@example.com
- ✅ Sample panic and SOS alerts
- ✅ Swagger UI for testing

## 🧪 Test Your First Endpoint

### Using Swagger UI
1. Go to `https://localhost:7000`
2. Find **POST /api/user/login**
3. Click **Try it out**
4. Use these credentials:
```json
{
  "email": "john.doe@example.com",
  "password": "password123"
}
```
5. Click **Execute**

### Using cURL
```bash
curl -X POST "https://localhost:7000/api/user/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "password123"
  }'
```

## 📝 Common Commands

### View all migrations
```bash
dotnet ef migrations list
```

### Rollback database
```bash
dotnet ef database update <PreviousMigrationName>
```

### Drop database (careful!)
```bash
dotnet ef database drop
```

### Rebuild project
```bash
dotnet clean
dotnet build
```

## 🐛 Troubleshooting

### Port Already in Use
Change the port in `Properties/launchSettings.json`:
```json
"applicationUrl": "https://localhost:7001;http://localhost:5001"
```

### Database Connection Failed
1. Verify PostgreSQL is running:
   ```bash
   # Windows (if installed as service)
   sc query postgresql-x64-15
   
   # Linux
   sudo systemctl status postgresql
   
   # macOS
   brew services list
   ```

2. Test connection:
   ```bash
   psql -U postgres -d safesphere_dev_db
   ```

### Migration Already Exists
```bash
# Remove last migration
dotnet ef migrations remove

# Or start fresh
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🎯 Next Steps

1. **Explore the API**
   - Try all endpoints in Swagger UI
   - Create new users and alerts
   - Test panic alert status updates

2. **Review the Code**
   - Check `Controllers/` for endpoint implementations
   - Review `Services/` for business logic
   - Examine `Models/` for entity structures

3. **Customize**
   - Add new properties to models
   - Create new endpoints
   - Modify validation rules

4. **Connect Frontend**
   - Use the API endpoints in your React Native app
   - Implement authentication flow
   - Display alerts on a map

## 📚 Documentation

- **Full Documentation**: See `README.md`
- **Project Structure**: See `PROJECT_STRUCTURE.md`
- **API Endpoints**: Swagger UI at `https://localhost:7000`

## 🆘 Need Help?

1. Check `logs/` folder for error details
2. Review `README.md` for comprehensive documentation
3. Verify PostgreSQL connection string
4. Ensure all NuGet packages are restored

---

**Happy Coding! 🚀**

