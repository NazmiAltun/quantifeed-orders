// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Extensions;
using Orders.Api.Services;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.LoadSettings();
builder.Services.AddOrdersServices(settings);
builder.Services.AddGrpc();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<OrdersService>();
});
await app.RunAsync();

namespace Orders.Api
{
    public partial class Program { }
}
