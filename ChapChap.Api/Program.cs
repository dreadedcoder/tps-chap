using ChapChap.Api.Models;
using Microsoft.AspNetCore.Mvc;
using ChapChap.Api.Extensions;


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
    .AddMassTransitWithRabbitMQ(massTransitOptions);

#endregion

#region configure request pipeline

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

#endregion

app.MapPost("/api/v1/transactions", 
async ([FromBody] TransactionRequest request, ILogger<Program> logger) =>
{
    logger.LogInformation("Received {@TransactionRequest} with {ReferenceId} and {UserId}",
        request, request.ReferenceId, request.UserId);

    try
    {
        if (request.Amount <= 0)
            return Results.BadRequest($"{nameof(request.Amount)} should be greater than 0");


        return Results.Ok();                                                                                 
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

