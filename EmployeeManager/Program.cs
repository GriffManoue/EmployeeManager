using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManager.Data;
using EmployeeManager.Model.LogicServices;
using EmployeeManager.Model.Interfaces;
using EmployeeManager.Model;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.DataAccess;

namespace EmployeeManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<EmployeeManagerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagerContext") ?? throw new InvalidOperationException("Connection string 'EmployeeManagerContext' not found.")));

            // Add services to the container.
            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<ILogicService<Employee>, EmployeeLogicService>();


            builder.Services.AddScoped<ILogicService<Department>, DepartmentLogicService>();      
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();

            builder.Services.AddControllers();
           

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           

            var app = builder.Build();

            InitializeDatabase(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        public static void InitializeDatabase(IHost app)
        {
            //todo exception handling
            using (var scope = app.Services.CreateScope()) {

                var services = scope.ServiceProvider;
                try { 
                
                    var context = services.GetRequiredService<EmployeeManagerContext>();
                    DbInitializer.Initialize(context);
                
                }catch(Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }

            }
            
        }
    }
}
