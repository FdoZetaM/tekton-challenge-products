using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using TektonChallengeProducts.Api.Middlewares;
using TektonChallengeProducts.Application;
using TektonChallengeProducts.Infrastructure;
using Json = System.Text.Json;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                }).AddControllers()
                .AddJsonOptions( options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = Json.JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                } );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
       .AddApplication(typeof(Program).Assembly)
       .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.ApplyPendingMigrations();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapControllers();

app.UseCors( options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );

await app.RunAsync();
