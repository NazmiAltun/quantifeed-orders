// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using Orders.Api.Models;

namespace Orders.Api.Validators;

public class OrdersRequestValidator : AbstractValidator<OrdersRequest>
{
    public OrdersRequestValidator(
        Func<BasketOrderValidator> basketOrderValidatorFactory)
    {
        RuleFor(x => x.Orders)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.OrderRequestCannotBeEmpty);
        RuleFor(x => x.Orders)
            .Must(OrdersMustHaveUniqueId!)
            .WithErrorCode(ErrorCodes.OrdersMustHaveUniqueId);
        RuleForEach(x => x.Orders)
            .SetValidator(basketOrderValidatorFactory());
    }

    private bool OrdersMustHaveUniqueId(IReadOnlyCollection<Order>? orders)
    {
        var orderSet = new HashSet<string>();

        foreach (var order in orders ?? Enumerable.Empty<Order?>())
        {
            if (order?.OrderId == null)
            {
                continue;
            }

            if (!orderSet.Add(order.OrderId))
            {
                return false;
            }

            foreach (var childOrder in order.ChildOrders ?? Enumerable.Empty<Order?>())
            {
                if (childOrder?.OrderId is not null &&
                    !orderSet.Add(childOrder.OrderId))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
