using Domain.Interfaces.Repository;
using Domain.Interfaces.Services;
using Domain.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Services;
using Infrastructure.Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IEmployeeProjectRepository, EmployeeProjectRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmployeeProjectService, EmployeeProjectService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeProjects API", Version = "v1" });
});

var app = builder.Build();
app.UseCors("AllowLocalhost3000"); // Enable the CORS policy

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();

    var policy = Policy
        .Handle<SqlException>()
        .WaitAndRetry(5, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), (exception, timeSpan, retryCount, context) =>
        {
            Console.WriteLine($"Retrying database migration: attempt {retryCount}...");
        });

    policy.Execute(() =>
    {
        dbContext.Database.Migrate();
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeProjects API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }