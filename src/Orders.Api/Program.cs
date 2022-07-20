// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Extensions;
using Orders.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.LoadSettings();

builder.Services.AddRedis(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddGrpc();
builder.Services.AddOrdersServices(settings);

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<OrdersService>();
});
await app.RunAsync();

namespace Orders.Api    // Hack for integration tests
{
    public partial class Program { }
}
