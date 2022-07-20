// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Models;

public class OrdersRequest
{
    public IReadOnlyCollection<BasketOrder>? Orders { get; set; }
}
