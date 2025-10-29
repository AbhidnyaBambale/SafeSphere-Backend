using Microsoft.EntityFrameworkCore;
using SafeSphere.API.Data;
using SafeSphere.API.Repositories;
using SafeSphere.API.Services;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/safesphere-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting SafeSphere API");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();

    // Configure Entity Framework Core with PostgreSQL
    builder.Services.AddDbContext<SafeSphereDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Register repositories
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAlertRepository, AlertRepository>();

    // Register services
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAlertService, AlertService>();

    // Add AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));

    // Configure CORS for React Native frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactNative", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Add Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "SafeSphere API",
            Version = "v1",
            Description = "API for SafeSphere mobile application - Emergency alerts and user management",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "SafeSphere Team",
                Email = "support@safesphere.com"
            }
        });

        // Enable XML comments if available
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeSphere API v1");
            c.RoutePrefix = string.Empty; // Set Swagger UI at app root
        });
    }

    app.UseHttpsRedirection();

    // Enable CORS
    app.UseCors("AllowReactNative");

    app.UseAuthorization();

    app.MapControllers();

    // Health check endpoint
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .WithTags("Health");

    Log.Information("SafeSphere API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
