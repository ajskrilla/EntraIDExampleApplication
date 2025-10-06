using Serilog;
using EntraIDScanner.API.RepositoryInterfaces.Entra;
using EntraIDScanner.API.RepositoryInterfaces.Mongo;
using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository;
using EntraIDScanner.API.Data.MongoDB;
using EntraIDScanner.API.Services.EntraId;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add MongoDB Configuration
//var mongoSettings = builder.Configuration.GetSection("MongoDB");

// Register Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{//allow all origins
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
//Mongo DB context add (DI)
builder.Services.AddSingleton<MongoDbContext>();

// for the dependency injection:
builder.Services.AddScoped<GraphService>();
builder.Services.AddScoped<CredentialService>();
builder.Services.AddScoped<SyncService>();
// Entra repo
builder.Services.AddScoped<IUserRepository, EntraUserRepository>();
builder.Services.AddScoped<IDeviceRepository, EntraDeviceRepository>();
// DB repo
builder.Services.AddScoped<IStoredDatabaseUserRepository, MongoUserRepository>();
builder.Services.AddScoped<IStoredDatabaseDeviceRepository, MongoDeviceRepository>();
//

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
