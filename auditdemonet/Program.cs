using Audit.Core;
using auditdemonet.Models;
using Microsoft.EntityFrameworkCore;

 
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);

// services

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AuditDemoDbContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(new Audit.EntityFramework.AuditSaveChangesInterceptor())
);

builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// audit storage
Audit.Core.Configuration.Setup()
    .UseSqlServer(cfg => cfg
        .ConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
        .TableName("AuditEvents")
        .IdColumnName("EventId")
        .JsonColumnName("Data"));

// audit ef core
Audit.EntityFramework.Configuration.Setup()
    .ForContext<AuditDemoDbContext>(cfg => cfg
        .IncludeEntityObjects());

var app = builder.Build();


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
