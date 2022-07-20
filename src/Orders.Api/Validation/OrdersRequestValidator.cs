// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using Orders.Api.Models;

namespace Orders.Api.Validation;

public class OrdersRequestValidator : AbstractValidator<OrdersRequest>
{
    public OrdersRequestValidator(
        Func<BasketOrderValidator> basketOrderValidatorFactory)
    {
        RuleFor(x => x.Orders)
            .NotEmpty()
            .WithMessage(ErrorMessages.OrderRequestCannotBeEmpty)
            .WithErrorCode(ErrorCodes.OrderRequestCannotBeEmpty);
        RuleFor(x => x.Orders)
            .Must(OrdersMustHaveUniqueIdWithinTheRequest)
            .WithMessage(ErrorMessages.OrdersMustHaveUniqueId)
            .WithErrorCode(ErrorCodes.OrdersMustHaveUniqueId);
        RuleForEach(x => x.Orders)
            .SetValidator(basketOrderValidatorFactory());
    }

    private static bool OrdersMustHaveUniqueIdWithinTheRequest(
        OrdersRequest request,
        IReadOnlyCollection<Order>? orders,
        ValidationContext<OrdersRequest> validationContext)
    {
        var orderSet = new HashSet<string>();
        var repeatingId = "";

        foreach (var order in orders ?? Enumerable.Empty<Order?>())
        {
            if (order?.OrderId == null)
            {
                continue;
            }

            if (!orderSet.Add(order.OrderId))
            {
                repeatingId = order.OrderId;
            }

            foreach (var childOrder in order.ChildOrders ?? Enumerable.Empty<Order?>())
            {
                if (childOrder?.OrderId is not null &&
                    !orderSet.Add(childOrder.OrderId))
                {
                    repeatingId = order.OrderId;
                }
            }
        }
        validationContext.MessageFormatter.AppendArgument("OrderId", repeatingId);

        return repeatingId == "";
    }
}
