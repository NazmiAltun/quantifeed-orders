// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Models;

namespace Orders.Api.Validators;

public class ClientRuleSettings
{
    public string? ClientId { get; set; }
    public OrderType Type { get; set; }
    public string? Currency { get; set; }
    public string? Destination { get; set; }
    public decimal MinimumChildNotionalAmount { get; set; }
    public decimal? MinimumBasketNotionalAmount { get; set; }
}
