// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Proto;
using Orders.Test.Common;

namespace Orders.Api.Stress.Test;

internal static class RequestGenerator
{
    public static OrdersRequest Generate()
    {
        var basketCount = DataGenerator.Next(2, 10);
        var stockOrderPerBasket = DataGenerator.Next(5, 20);

        return OrdersRequestMother.Create(basketCount, stockOrderPerBasket);
    }
}
