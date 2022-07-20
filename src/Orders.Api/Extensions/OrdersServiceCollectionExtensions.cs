// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Nodes;
using Orders.Api.Repositories;
using Orders.Api.Validators;
using StackExchange.Redis;

namespace Orders.Api.Extensions;

public static class OrdersServiceCollectionExtensions
{
    private const string CurrenciesDataSourceFile = "currencies.json";

    public static IServiceCollection AddOrdersServices(
        this IServiceCollection serviceCollection,
        OrdersApiSettings settings)
    {
        var currencyCodes = LoadCurrencyCodes();

        serviceCollection.AddSingleton(sp =>
            new OrderValidator(settings.ClientRuleSettings!, currencyCodes));

        serviceCollection.AddSingleton(sp =>
            new BasketOrderValidator(
                sp.GetRequiredService<OrderValidator>,
                settings.ClientRuleSettings!,
                settings.BasketOrderChildSumWeight));

        serviceCollection.AddSingleton(sp =>
            new OrdersRequestValidator(sp.GetRequiredService<BasketOrderValidator>));

        serviceCollection.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(settings.RedisConnectionString!));

        serviceCollection.AddSingleton<OrdersRepository>();

        return serviceCollection;
    }

    private static HashSet<string> LoadCurrencyCodes()
    {
        var json = File.ReadAllText(CurrenciesDataSourceFile);
        var currencies = JsonSerializer.Deserialize<JsonObject[]>(json);
        return currencies!.Select(c => c["code"]!.ToString()).ToHashSet();
    }
}
