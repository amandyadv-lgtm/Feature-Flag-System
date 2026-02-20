using FeatureFlag.AdminAPI.Data;
using Microsoft.EntityFrameworkCore;
using FeatureFlag.AdminAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 1. Add SignalR
builder.Services.AddSignalR();


// 2. Add Database (using In-Memory for now to test quickly, change to SQL Server string later)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("FeatureFlagDb"));

// 3. Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Add CORS (Crucial for Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") //angular endpoint
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); //Needed for signalR
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers(); 
app.UseCors("AllowAll");

// 5. Map the SignalR Hub Endpoint
app.MapHub<FlagHub>("/flaghub");


app.Run();

