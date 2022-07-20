// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Helpers;
using Orders.Api.Models;
using StackExchange.Redis;

namespace Orders.Api.Repositories;

public class OrdersRepository
{
    private const string SaveOrderScript = @"
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
        var (keys, values) = RedisMapper.MapToRedisKeyValuePairs(basketOrders);
        var result = (RedisResult[]?)await _database.ScriptEvaluateAsync(SaveOrderScript, keys, values);

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
}
