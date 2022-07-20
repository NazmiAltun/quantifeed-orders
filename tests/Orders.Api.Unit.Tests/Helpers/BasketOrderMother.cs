// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Test.Common;

namespace Orders.Api.Unit.Tests.Helpers;

internal static class BasketOrderMother
{
    public static BasketOrder Create(Action<BasketOrder> with)
    {
        var basketOrder = Create();

        with(basketOrder);

        return basketOrder;
    }

    public static BasketOrder Create()
    {
        var basketOrder = new BasketOrder
        {
            OrderId = DataGenerator.Id(),
            Type = DataGenerator.OrderType(),
            Currency = DataGenerator.Currency(),
            ClientId = DataGenerator.ClientId(),
            Destination = DataGenerator.Destination(),
            Symbol = DataGenerator.Symbol(),
            ChildOrders = new List<Order>
            {
                OrderMother.Create(o => o.Weight = 0.5m),
                OrderMother.Create(o => o.Weight = 0.4m),
                OrderMother.Create(o => o.Weight = 0.1m),
            }
        };

        return basketOrder;
    }
}
