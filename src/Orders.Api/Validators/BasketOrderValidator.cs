// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using Orders.Api.Models;

namespace Orders.Api.Validators;

public class BasketOrderValidator : AbstractValidator<BasketOrder>
{
    public BasketOrderValidator(
        Func<OrderValidator> orderValidatorFactory,
        IEnumerable<ClientRuleSettings> clientSpecificRuleSettings,
        decimal expectedChildSumWeight)
    {
        Include(orderValidatorFactory());
        CreateRuleForChildOrders();
        CreateRuleForWeight(expectedChildSumWeight);
        CreateClientSpecificRules(clientSpecificRuleSettings);
        SetChildOrderValidator(orderValidatorFactory);
    }

    private void SetChildOrderValidator(Func<OrderValidator> orderValidatorFactory)
    {
        RuleForEach(x => x.ChildOrders)
            .SetValidator(orderValidatorFactory());
    }

    private void CreateRuleForChildOrders()
    {
        RuleFor(o => o.ChildOrders)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.BasketOrderMustHaveChildOrders);
    }

    private void CreateRuleForWeight(decimal expectedChildSumWeight)
    {
        RuleFor(o => o.Weight)
            .Equal(expectedChildSumWeight)
            .WithErrorCode(ErrorCodes.InvalidChildOrderWeightSum);
    }

    private void CreateClientSpecificRules(IEnumerable<ClientRuleSettings> clientSpecificRuleSettings)
    {
        foreach (var settings in clientSpecificRuleSettings)
        {
            When(o => o.ClientId == settings.ClientId && o.ClientId != null, () =>
            {
                if (settings.MinimumBasketNotionalAmount != null)
                {
                    RuleFor(o => o.NotionalAmount)
                        .GreaterThanOrEqualTo(settings.MinimumBasketNotionalAmount.Value)
                        .WithErrorCode(ErrorCodes.BasketNotionalAmountBelowClientsMinimum);
                }
            });
        }
    }
}
