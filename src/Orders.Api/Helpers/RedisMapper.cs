// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Orders.Api.Models;
using StackExchange.Redis;

namespace Orders.Api.Helpers;

internal static class RedisMapper
{
    public static (RedisKey[], RedisValue[]) MapToRedisKeyValuePairs(IReadOnlyCollection<BasketOrder> basketOrders)
    {
        var flattenOrders = MapToFlattenOrders(basketOrders).ToArray();
        var keys = new RedisKey[flattenOrders.Length];
        var values = new RedisValue[flattenOrders.Length];

        for (var i = 0; i < flattenOrders.Length; i++)
        {
            keys[i] = flattenOrders[i].OrderId;
            values[i] = JsonSerializer.Serialize(flattenOrders[i]);
        }

        return (keys, values);
    }

    private static IEnumerable<FlattenOrder> MapToFlattenOrders(IReadOnlyCollection<BasketOrder> basketOrders)
    {
        return basketOrders.SelectMany(o => o.ChildOrders!)
            .Select(MapToFlattenOrder)
            .Concat(basketOrders.Select(MapToFlattenOrder));
    }

    private static FlattenOrder MapToFlattenOrder(Models.Order order)
    {
        return new FlattenOrder(
            order.OrderId!,
            order.Type.ToString(),
            order.Currency!,
            order.Symbol!,
            order.Destination!,
            order.ClientId!,
            order.Weight,
            order.NotionalAmount,
            order.ChildOrders?.Select(o => o.OrderId!).ToArray());
    }

    private record FlattenOrder(
        string OrderId,
        string Type,
        string Currency,
        string Symbol,
        string Destination,
        string ClientId,
        decimal Weight,
        decimal NotionalAmount,
        string[]? ChildOrderIds);
}
