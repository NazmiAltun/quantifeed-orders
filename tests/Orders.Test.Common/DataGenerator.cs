// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Models;

namespace Orders.Test.Common;

public static class DataGenerator
{
    private static readonly string[] Symbols = { "$", "€", "₺" };
    private static int _seed = Environment.TickCount;
    private static readonly ThreadLocal<Random> Random = new(() => new Random(Interlocked.Increment(ref _seed)));
    public static readonly string[] Currencies = {
        "USD",
        "HKD",
        "EUR",
        "TRY",
        "CNY",
        "AUD"
    };

    public static string Id() => Guid.NewGuid().ToString();
    public static OrderType OrderType() => (OrderType)Random.Value!.Next(1, 3);
    public static string Currency() => Currencies[Random.Value!.Next(Currencies.Length)];
    public static string ClientId() => $"Client_{Id()}";
    public static string Destination() => $"Destination_{Id()}";
    public static string Symbol() => Symbols[Random.Value!.Next(Symbols.Length)];
    public static decimal NotionalAmount() => Random.Value!.Next(10, 1000);
    public static decimal Weight() => Random.Value!.Next(1, 100) / 100m;
    public static int Next(int min,int max) => Random.Value!.Next(min, max);
}
