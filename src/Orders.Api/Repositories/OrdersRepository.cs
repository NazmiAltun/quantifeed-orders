// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Orders.Api.Models;
using StackExchange.Redis;
using Order = Orders.Api.Models.Order;

namespace Orders.Api.Repositories;

public class OrdersRepository
{
    private const string OrderSaveScript = @"
        local result = {}
        local failed = false
        for i=1, #KEYS do
            result[i] = redis.call('HSETNX',KEYS[i],'order',ARGV[i])
            if( result[i] == 0 ) then
                failed = true
            end
        end
        if( failed ) -- rollback the whole request if any of the orders failed
        then
            for i=1, #KEYS do
               redis.call('HDEL',KEYS[i],'order')
            end
        end
        return result
    ";
    private readonly IDatabase _database;

    public OrdersRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<SaveResult> SaveOrdersAsync(IReadOnlyCollection<BasketOrder> basketOrders)
    {
        var (keys, values) = MapToRedisKeyValuePairs(basketOrders);
        var result = (RedisResult[]?)await _database.ScriptEvaluateAsync(OrderSaveScript, keys, values);

        var existingOrderIds = new List<string>();

        for (var i = 0; i < result!.Length; i++)
        {
            if ((int)result[i] == 0)
            {
                existingOrderIds.Add(keys[i].ToString());
            }
        }

        return new SaveResult(existingOrderIds.Count == 0, existingOrderIds);
    }

    private static (RedisKey[], RedisValue[]) MapToRedisKeyValuePairs(IReadOnlyCollection<BasketOrder> basketOrders)
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

    private static FlattenOrder MapToFlattenOrder(Order order)
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
