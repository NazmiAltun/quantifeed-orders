// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Validation;

public static class ErrorCodes
{
    public const string OrderRequestCannotBeEmpty = "ORDER_REQUEST_MUST_HAVE_AT_LEAST_1_ORDER";
    public const string OrdersMustHaveUniqueId = "ORDERS_MUST_HAVE_UNIQUE_ID";
    public const string OrderIdAlreadyExists = "ORDER_ID_ALREADY_EXISTS";
    public const string OrderIdCannotBeEmpty = "ORDER_ID_CANNOT_BE_EMPTY";
    public const string DestinationCannotBeEmpty = "DESTINATION_CANNOT_BE_EMPTY";
    public const string ClientIdCannotBeEmpty = "CLIENT_ID_CANNOT_BE_EMPTY";
    public const string SymbolCannotBeEmpty = "SYMBOL_CANNOT_BE_EMPTY";
    public const string InvalidCurrencyCode = "INVALID_CURRENCY_CODE";
    public const string InvalidNotionalAmount = "INVALID_NOTIONAL_AMOUNT";
    public const string WeightIsOutOfRange = "WEIGHT_IS_OUT_OF_RANGE";
    public const string BasketOrderMustHaveChildOrders = "BASKET_ORDER_MUST_HAVE_CHILD_ORDERS";
    public const string InvalidChildOrderWeightSum = "INVALID_CHILD_ORDER_WEIGHT_SUM";
    public const string ClientOrderTypeMismatch = "CLIENT_ORDER_TYPE_MISMATCH";
    public const string ClientOrderCurrencyMismatch = "CLIENT_ORDER_CURRENCY_MISMATCH";
    public const string ClientDestinationMismatch = "CLIENT_DESTINATION_MISMATCH";
    public const string ChildOrderNotionalAmountBelowClientsMinimum = "CHILD_ORDER_NOTIONAL_AMOUNT_BELOW_CLIENTS_MINIMUM";
    public const string BasketNotionalAmountBelowClientsMinimum = "BASKET_NOTIONAL_AMOUNT_BELOW_CLIENTS_MINIMUM";
}
