using ChapChap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using ChapChap.Api.Extensions;
using MassTransit;
using ChapChap.Consumers;
using ChapChap.Consumers.Extensions;


var builder = WebApplication.CreateBuilder(args);

#region add services 
builder
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var massTransitOptions = new MassTransitOptions();
builder.Configuration.Bind("MassTransit", massTransitOptions);

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
/// Minimal API endpoint to handle POST requests to "/example".
/// </summary>
/// <param name="request">The request data sent by the client.</param>
/// <returns>Returns an appropriate response based on the request.</returns>
app.MapPost("/transactions", 
async ([FromBody] TransactionRequest request, ILogger<Program> logger,
    IBus massTransitBus ) =>
{
    logger.LogInformation("Received {@TransactionRequest} with {ReferenceId} and {UserId}",
        request, request.ReferenceId, request.UserId);

    try
    {
        //vallidate incoming request
        if (request.Amount <= 0)
            return Results.BadRequest($"{nameof(request.Amount)} should be greater than 0");

        //set up queue and send the request to consumer
        var rabbitMqOptions = massTransitOptions.RabbitMQ ?? 
            throw new ArgumentNullException(nameof(massTransitOptions.RabbitMQ));

        var address = $"{rabbitMqOptions.Host}/{rabbitMqOptions.TransactionQueue}";

        var endpoint = await massTransitBus.GetSendEndpoint(new Uri(address));
        await endpoint.Send(request);

        return Results.Ok("Transaction request queued for processing");                                                                                 
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occured while processing the request with {ReferenceId} and {UserId}",
            request.ReferenceId, request.UserId);

        return Results.StatusCode(504);
    }
})
.WithName("PostTransaction")
.WithOpenApi();

app.Run();

