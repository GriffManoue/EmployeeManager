using EmployeeManager.Data;
using EmployeeManager.DataAccess;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using EmployeeManager.Model.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager;

/// <summary>
/// The main entry point for the EmployeeManager application.
/// </summary>
public class Program
{
    /// <summary>
    /// The main method that starts the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Configures the database context for the application using SQL Server with a connection string.
        builder.Services.AddDbContext<EmployeeManagerContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagerContext") ??
                                 throw new InvalidOperationException(
                                     "Connection string 'EmployeeManagerContext' not found.")));

        // Registers services and repositories for dependency injection.
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
        builder.Services.AddScoped<ILogicService<Employee>, EmployeeLogicService>();
        builder.Services.AddScoped<ILogicService<Department>, DepartmentLogicService>();
        builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
        builder.Services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
        builder.Services.AddScoped<IPasswordService<Employee>, PasswordService>();

        // Adds MVC controllers to the application.
        builder.Services.AddControllers();

        // Adds Swagger generation for API documentation.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Initializes the database with default values if necessary.
        InitializeDatabase(app);

        // Configures middleware for development environment, including Swagger UI.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Configures middleware for HTTPs redirection and authorization.
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        // Runs the application.
        app.Run();
    }

    /// <summary>
    /// Initializes the database with default values.
    /// </summary>
    /// <param name="app">The host application.</param>
    public static void InitializeDatabase(IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<EmployeeManagerContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}