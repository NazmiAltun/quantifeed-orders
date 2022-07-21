// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Grpc.Net.Client;
using Orders.Proto;

namespace Orders.Api.Stress.Test;

public static class Orchestrator
{
    private static readonly OrderService.OrderServiceClient Client;

    static Orchestrator()
    {
        //TODO: Dispose?
        var channel = GrpcChannel.ForAddress(Configuration.Url, new GrpcChannelOptions
        {
            HttpHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
            }
        });
        Client = new OrderService.OrderServiceClient(channel);
    }

    public static async Task StressAsync()
    {
        var step = Configuration.StartRequestCount;

        for (var count = Configuration.StartRequestCount;
             count < Configuration.TotalRequestCount;
             count += step)
        {
            Stats.StartStep();
            var calls = Enumerable.Range(0, step)
                .Select(async _ => await SendRequestAsync())
                .ToArray();
            await Task.WhenAll(calls);

            Console.Write($"Step: {step} ");
            Stats.EndStep(step);
            Stats.Display();
            step += Configuration.RampUpRequestCount;
        }
    }

    private static async Task SendRequestAsync()
    {
        try
        {
            var response = await Client.ProcessOrdersAsync(RequestGenerator.Generate());

            if (response.Successful)
            {
                Stats.IncrementSuccess();
            }
            else
            {
                Console.WriteLine(JsonSerializer.Serialize(response));
                Stats.IncrementFail();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Stats.IncrementFail();
        }
    }
}
