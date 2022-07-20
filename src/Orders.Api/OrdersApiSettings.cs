// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Validation;

namespace Orders.Api;

public class OrdersApiSettings
{
    public ClientRuleSettings[]? ClientRuleSettings { get; set; }
    public decimal BasketOrderChildSumWeight { get; set; }
}
