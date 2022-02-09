using Health.Patient.Api.Middleware;
using Health.Patient.Domain.Core;
using Health.Patient.Storage.Sql.Core;
using Health.Patient.Transport.Api.Core;

var builder = WebApplication.CreateBuilder(args);

// var preBuildConfig = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json", optional: false)
//     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
//     .AddEnvironmentVariables()
//     .Build();

// Add Configuration to the container
builder.Configuration.AddJsonFile("appsettings.json", optional: false ,reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container for API
var apiSettings = builder.Configuration.GetSection("ApiConfiguration").Get<ApiConfiguration>();
builder.Services.AddSingleton<IApiConfiguration>(apiSettings);

builder.Services.AddControllers();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSingleton<Health.Patient.Api.Core.Serialization.IJsonSerializer, Health.Patient.Api.Core.Serialization.JsonSerializer>();

// Add Services to Domain (and storage dependant service)
var storageSettings = builder.Configuration.GetSection("DomainConfiguration:StorageConfiguration:PatientDatabase").Get<SqlDatabaseConfiguration>();
builder.Services.AddDomainServices(new DomainConfiguration(new StorageConfiguration(storageSettings)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();