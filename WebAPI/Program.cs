using Application;
using Persistence;
using Shared;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregar servicios Application
#region Application
builder.Services.AddApplication();
#endregion
#region Infraestructure
builder.Services.AddPersistenceInfraestructure(builder.Configuration);
#endregion
#region SharedInfraestructure
builder.Services.AddSharedInfraestructure(builder.Configuration);
#endregion
#region Version
builder.Services.AddApiVersioningExtension();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.useErrorHandlingMiddleware();

app.MapControllers();

app.Run();
