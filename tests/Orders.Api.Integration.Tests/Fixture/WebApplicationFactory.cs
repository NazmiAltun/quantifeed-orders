// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Orders.Proto;
using StackExchange.Redis;

namespace Orders.Api.Integration.Tests.Fixture;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    public OrderService.OrderServiceClient CreateGrpcClient()
    {
        var channel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions()
        {
            HttpClient = CreateClient()
        });
        return new OrderService.OrderServiceClient(channel);
    }

    public IConnectionMultiplexer GetRedis()
    {
        return (Services.GetService(typeof(IConnectionMultiplexer)) as IConnectionMultiplexer)!;
    }
}
