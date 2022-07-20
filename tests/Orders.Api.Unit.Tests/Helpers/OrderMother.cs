// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Test.Common;

namespace Orders.Api.Unit.Tests.Helpers;

internal static class OrderMother
{
    public static Order Create(Action<Order> with)
    {
        var order = Create();

        with(order);

        return order;
    }

    public static Order Create()
    {
        return new Order
        {
            OrderId = DataGenerator.Id(),
            Type = DataGenerator.OrderType(),
            Currency = DataGenerator.Currency(),
            ClientId = DataGenerator.ClientId(),
            Destination = DataGenerator.Destination(),
            Symbol = DataGenerator.Symbol(),
            NotionalAmount = DataGenerator.NotionalAmount(),
            Weight = DataGenerator.Weight()
        };
    }
}
