// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Proto;

namespace Orders.Test.Common;

public class OrdersRequestMother
{
    public static OrdersRequest Create(
        Action<OrdersRequest> with,
        int basketOrderCount = 3,
        int stockOrderPerBasket = 4)
    {
        var request = Create(basketOrderCount, stockOrderPerBasket);
        with(request);

        return request;
    }

    public static OrdersRequest Create(
        int basketOrderCount = 3,
        int stockOrderPerBasket = 4)
    {
        var request = new OrdersRequest();
        var basketOrders = Enumerable.Range(0, basketOrderCount)
            .Select(_ => CreateBasketOrder(stockOrderPerBasket))
            .ToArray();
        request.Orders.AddRange(basketOrders);

        return request;
    }

    private static Order CreateBasketOrder(int childOrderCount)
    {
        var order = new Order
        {
            OrderId = DataGenerator.Id(),
            Type = (OrderType)DataGenerator.OrderType(),
            Currency = DataGenerator.Currency(),
            ClientId = DataGenerator.ClientId(),
            Destination = DataGenerator.Destination(),
            Symbol = DataGenerator.Symbol(),
        };
        var childOrders = Enumerable.Range(0, childOrderCount)
            .Select(_ => CreateOrder(o => o.Weight = 1.0 / childOrderCount))
            .ToArray();

        order.ChildOrders.AddRange(childOrders);

        return order;
    }

    private static Order CreateOrder(Action<Order> with)
    {
        var order = new Order
        {
            OrderId = DataGenerator.Id(),
            Type = (OrderType)DataGenerator.OrderType(),
            Currency = DataGenerator.Currency(),
            ClientId = DataGenerator.ClientId(),
            Destination = DataGenerator.Destination(),
            Symbol = DataGenerator.Symbol(),
            NotionalAmount = (double)DataGenerator.NotionalAmount(),
            Weight = (double)DataGenerator.Weight()
        };
        with(order);

        return order;
    }
}
