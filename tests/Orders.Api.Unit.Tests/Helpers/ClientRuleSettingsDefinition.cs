// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Validation;

namespace Orders.Api.Unit.Tests.Helpers;

internal static class ClientRuleSettingsDefinition
{
    public static IEnumerable<ClientRuleSettings> All
    {
        get
        {
            yield return ClientA;
            yield return ClientB;
        }
    }

    public static readonly ClientRuleSettings ClientA = new()
    {
        ClientId = "ClientId A",
        Type = OrderType.Market,
        Currency = "HKD",
        Destination = "DestinationA",
        MinimumChildNotionalAmount = 100
    };

    public static readonly ClientRuleSettings ClientB = new()
    {
        ClientId = "ClientId B",
        Type = OrderType.Limit,
        Currency = "USD",
        Destination = "DestinationB",
        MinimumChildNotionalAmount = 100,
        MinimumBasketNotionalAmount = 10000
    };
}
