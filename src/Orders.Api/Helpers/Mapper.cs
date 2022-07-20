// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation.Results;
using Orders.Api.Extensions;
using Orders.Api.Models;
using Orders.Api.Repositories;
using Orders.Api.Validation;
using Orders.Proto;
using OrdersRequest = Orders.Proto.OrdersRequest;
using OrderType = Orders.Api.Models.OrderType;
using ValidationResult = Orders.Proto.ValidationResult;

namespace Orders.Api.Helpers;

internal static class Mapper
{
    private static readonly ProcessOrdersResponse SuccessfulResponse = new()
    {
        Successful = true
    };

    public static Models.OrdersRequest MapToOrderRequestModel(OrdersRequest request)
    {
        return new Models.OrdersRequest
        {
            Orders = request.Orders.Select(o => new BasketOrder
            {
                OrderId = o.OrderId,
                Type = (OrderType)o.Type,
                Currency = o.Currency,
                Symbol = o.Symbol,
                Destination = o.Destination,
                ClientId = o.ClientId,
                ChildOrders = o.ChildOrders.Select(co => new Models.Order
                {
                    OrderId = co.OrderId,
                    Type = (OrderType)co.Type,
                    Currency = co.Currency,
                    Symbol = co.Symbol,
                    Destination = co.Destination,
                    ClientId = co.ClientId,
                    Weight = (decimal)co.Weight,
                    NotionalAmount = (decimal)co.NotionalAmount,

                }).ToArray(),
            }).ToArray()
        };
    }

    public static ProcessOrdersResponse MapToProcessOrdersResponse(IEnumerable<ValidationFailure> validationFailures)
    {
        var response = new ProcessOrdersResponse() { Successful = false };
        var orderValidationResults = validationFailures.Select(failure => new ValidationResult
        {
            PropertyName = failure.PropertyName,
            ErrorCode = failure.ErrorCode,
            ErrorMessage = failure.ErrorMessage,
            AttemptedValue = (string?)failure.AttemptedValue ?? ""
        });

        response.ValidationResults.AddRange(orderValidationResults);

        return response;
    }

    public static ProcessOrdersResponse MapToProcessOrdersResponse(SaveResult saveResult)
    {
        if (saveResult.IsSuccessful)
        {
            return SuccessfulResponse;
        }
        var response = new ProcessOrdersResponse();
        var orderValidationResults = saveResult.ExistingOrderIds.Select(id => new ValidationResult
        {
            ErrorMessage = ErrorMessages.OrderIdAlreadyExists.Format(id),
            ErrorCode = ErrorCodes.OrderIdAlreadyExists,
            AttemptedValue = id,
        });
        response.ValidationResults.AddRange(orderValidationResults);

        return response;
    }
}
