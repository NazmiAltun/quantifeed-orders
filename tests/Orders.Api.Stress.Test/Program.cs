using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Orders.Api.Stress.Test;
using Orders.Proto;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

var config = configuration.Build();
var url = config["GrpcApiUrl"];

Console.WriteLine($"Stressing started on host {url}");
using var channel = GrpcChannel.ForAddress(url);
var client = new OrderService.OrderServiceClient(channel);

const int step = 1000;

for(var i = 0 ; i < int.MaxValue ; i += step)
{
    var calls = Enumerable.Range(0, step)
    .Select(async _ => await client.ProcessOrdersAsync(RequestGenerator.Generate()))
    .ToArray();
    await Task.WhenAll(calls);
    Console.WriteLine($"Done: {i}");
}

Console.WriteLine("Stressing completed...");
