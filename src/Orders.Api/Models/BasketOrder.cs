// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Models;

public class BasketOrder : Order
{
    //// Requirement is not clear. Does a basket order have its own weight ? or the weight of basket order is
    //// the sum of its child orders weights? 
    public override decimal Weight => ChildOrders?.Sum(o => o.Weight) ?? 0;
    //// Requirement is not clear. Does a basket order have its notional amount ? or is it sum of its child orders?
    public override decimal NotionalAmount => ChildOrders?.Sum(o => o.NotionalAmount) ?? 0;
}
