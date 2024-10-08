using ChapChap.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddGrpc();

var app = builder.Build();

app.MapGrpcService<MakePaymentService>();
app.MapGet("/", () => "Requests must come from a gRPC client");

app.Run();
