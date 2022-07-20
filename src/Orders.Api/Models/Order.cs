// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Models;

public class Order
{
    public string? OrderId { get; set; }
    public OrderType Type { get; set; }
    public string? Currency { get; set; }
    public string? Symbol { get; set; }
    public string? Destination { get; set; }
    public string? ClientId { get; set; }
    public virtual decimal Weight { get; set; }
    public virtual decimal NotionalAmount { get; set; }
    public IReadOnlyCollection<Order>? ChildOrders { get; set; }
}
