// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Validation;
using Orders.Test.Common;

namespace Orders.Api.Unit.Tests.Helpers;

internal static class ValidatorFactory
{
    public static OrderValidator CreateOrderValidator() =>
        new(ClientRuleSettingsDefinition.All, DataGenerator.Currencies.ToHashSet());

    public static BasketOrderValidator CreateBasketOrderValidator() =>
        new(CreateOrderValidator, ClientRuleSettingsDefinition.All, 1);

    public static OrdersRequestValidator CreateOrdersRequestValidator() =>
        new(CreateBasketOrderValidator);
}
