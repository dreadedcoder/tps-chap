using ChapChap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using ChapChap.Api.Extensions;
using ChapChap.Consumers;
using ChapChap.Consumers.Extensions;
using ChapChap.Api.Services;


var builder = WebApplication.CreateBuilder(args);

#region add services 
builder
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var massTransitOptions = new MassTransitOptions();
builder.Configuration.Bind("MassTransit", massTransitOptions);

builder
    .Services
    .AddSingleton(massTransitOptions)
    .AddTransient<TransactionRequestService>();

var consumersConfig = new ConsumersConfiguration();
builder.Configuration.Bind("Consumers", consumersConfig);

builder
    .Services
    .AddConsumersServices(consumersConfig)
    .AddMassTransitConsumersWithRabbitMQ(massTransitOptions);

#endregion

#region configure request pipeline

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

#endregion

/// <summary>
/// Minimal API endpoint to handle POST requests to "/example."
/// </summary>
/// <param name="request">The request data sent by the client.</param>
/// <returns>Returns an appropriate response based on the request.</returns>
app.MapPost("/transactions", 
async ([FromBody] TransactionRequest request, TransactionRequestService transactionRequestService,
        ILogger<Program> logger) =>
{
    try
    {
        return await transactionRequestService.ProcessTransactionRequestAsync(request);                                                                    
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occured while processing the request with ReferenceId {ReferenceId} " +
            "and UserId {UserId}",request.ReferenceId, request.UserId);

        return Results.StatusCode(504);
    }
})
.WithName("PostTransaction")
.WithOpenApi();

app.Run();

