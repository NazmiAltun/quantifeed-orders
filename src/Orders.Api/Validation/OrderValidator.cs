// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using FluentValidation;
using Orders.Api.Extensions;
using Orders.Api.Models;

namespace Orders.Api.Validation;

public class OrderValidator : AbstractValidator<Order>
{
    private readonly HashSet<string> _currencyCodes;

    public OrderValidator(
        IEnumerable<ClientRuleSettings> clientSpecificRuleSettings,
        HashSet<string> currencyCodes)
    {
        _currencyCodes = currencyCodes;
        CreateRuleForOrderId();
        CreateRuleForCurrency();
        CreateRuleForSymbol();
        CreateRuleForNotionalAmount();
        CreateRuleForDestination();
        CreateRuleForClientId();
        CreateRuleForWeight();
        CreateClientSpecificRules(clientSpecificRuleSettings);
    }

    private void CreateRuleForOrderId()
    {
        RuleFor(o => o.OrderId)
            .NotEmpty()
            .WithMessage(ErrorMessages.OrderIdCannotBeEmpty)
            .WithErrorCode(ErrorCodes.OrderIdCannotBeEmpty);
    }

    private void CreateRuleForCurrency()
    {
        RuleFor(o => o.Currency)
            .Must(BeValidCurrencyCode)
            .WithErrorCode(ErrorCodes.InvalidCurrencyCode);
    }

    private void CreateRuleForSymbol()
    {
        RuleFor(o => o.Symbol)
            .NotEmpty()
            .WithMessage(o => ErrorMessages.SymbolCannotBeEmpty.Format(o.OrderId))
            .WithErrorCode(ErrorCodes.SymbolCannotBeEmpty);
    }

    private void CreateRuleForNotionalAmount()
    {
        RuleFor(o => o.NotionalAmount)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodes.InvalidNotionalAmount);
    }

    private void CreateRuleForDestination()
    {
        RuleFor(o => o.Destination)
            .NotEmpty()
            .WithMessage(o => ErrorMessages.DestinationCannotBeEmpty.Format(o.OrderId))
            .WithErrorCode(ErrorCodes.DestinationCannotBeEmpty);
    }

    private void CreateRuleForClientId()
    {
        RuleFor(o => o.ClientId)
            .NotEmpty()
            .WithMessage(o => ErrorMessages.ClientIdCannotBeEmpty.Format(o.OrderId))
            .WithErrorCode(ErrorCodes.ClientIdCannotBeEmpty);
    }

    private void CreateRuleForWeight()
    {
        RuleFor(o => o.Weight)
            .InclusiveBetween(0, 1)
            .WithErrorCode(ErrorCodes.WeightIsOutOfRange);

        RuleFor(o => o.Weight)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodes.WeightIsOutOfRange);
    }

    private bool BeValidCurrencyCode(string? currency)
    {
        return !string.IsNullOrWhiteSpace(currency) &&
               currency.Length == 3 &&
               _currencyCodes.Contains(currency);
    }

    private void CreateClientSpecificRules(IEnumerable<ClientRuleSettings> clientSpecificRuleSettings)
    {
        foreach (var settings in clientSpecificRuleSettings)
        {
            When(o => o.ClientId == settings.ClientId && o.ClientId != null, () =>
            {
                CreateClientOrderTypeRule(settings);
                CreateClientCurrencyRule(settings);
                CreateClientDestinationRule(settings);
                CreateClientMinimumNotionalAmountRule(settings);
            });
        }
    }

    private void CreateClientOrderTypeRule(ClientRuleSettings settings)
    {
        RuleFor(o => o.Type)
            .Equal(settings.Type)
            .WithErrorCode(ErrorCodes.ClientOrderTypeMismatch);
    }

    private void CreateClientCurrencyRule(ClientRuleSettings settings)
    {
        if (settings.Currency != null)
        {
            RuleFor(o => o.Currency)
                .Equal(settings.Currency)
                .WithErrorCode(ErrorCodes.ClientOrderCurrencyMismatch);
        }
    }

    private void CreateClientMinimumNotionalAmountRule(ClientRuleSettings settings)
    {
        if (settings.MinimumChildNotionalAmount > 0)
        {
            RuleFor(o => o.NotionalAmount)
                .Equal(settings.MinimumChildNotionalAmount)
                .WithErrorCode(ErrorCodes.ChildOrderNotionalAmountBelowClientsMinimum);
        }
    }

    private void CreateClientDestinationRule(ClientRuleSettings settings)
    {
        if (settings.Destination != null)
        {
            RuleFor(o => o.Destination)
                .Equal(settings.Destination)
                .WithErrorCode(ErrorCodes.ClientDestinationMismatch);
        }
    }
}
