// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Validation;

public static class ErrorMessages
{
    public const string OrderRequestCannotBeEmpty = "Request should contain at least 1 basket order";
    public const string OrdersMustHaveUniqueId = "All the orders must have unique ID.Non-unique ID found: '{OrderId}'";
    public const string OrderIdAlreadyExists = "Order with ID :'{0}' already exists";
    public const string OrderIdCannotBeEmpty = "All the orders must have an ID";
    public const string DestinationCannotBeEmpty = "Order destination cannot be empty.Please check order with ID: '{0}'";
    public const string ClientIdCannotBeEmpty = "Order clientId cannot be empty.Please check order with ID: '{0}'";
    public const string SymbolCannotBeEmpty = "Orders must have a valid symbol.Please check order with ID: '{0}'";
}
