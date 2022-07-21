// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Proto;
using Orders.Test.Common;

namespace Orders.Api.Stress.Test;

internal static class RequestGenerator
{
    private static volatile OrdersRequest _lastRequest;

    public static OrdersRequest Generate()
    {
        var randomNo = DataGenerator.Next(1, 101);

        if (randomNo <= Configuration.InvalidRequestPercent)
        {
            return OrdersRequestMother.Create(o =>
            {
                o.Orders.First().ClientId = "";
                o.Orders.First().ChildOrders.First().Weight = 2;
                o.Orders.First().ChildOrders.Last().NotionalAmount = int.MaxValue;
            });
        }

        if (randomNo > Configuration.InvalidRequestPercent &&
            randomNo <= Configuration.InvalidRequestPercent + Configuration.ExistingIdRequestPercent)
        {
            return _lastRequest;
        }

        _lastRequest = OrdersRequestMother.Create();
        return _lastRequest;
    }
}
