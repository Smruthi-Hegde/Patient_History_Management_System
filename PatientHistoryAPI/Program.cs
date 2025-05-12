using Microsoft.EntityFrameworkCore;
using PatientHistoryAPI.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Prevent circular reference during serialization.
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Register DbContext with SQL Server
builder.Services.AddDbContext<PatientHistoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add Swagger generation for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS (allow everything for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
         policy.WithOrigins("http://localhost:5268")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable HTTPS redirection (for security)
app.UseHttpsRedirection();

// Enable CORS for all controllers
app.UseCors("AllowBlazorClient");

// Enable authorization middleware (for security/authentication)
app.UseAuthorization();

// Map the controllers to the API endpoints
app.MapControllers();

// Run the application
app.Run();
