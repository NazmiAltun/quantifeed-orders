// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(ClientOrderTypeMismatchData))]
    [Trait("Category", "Unit")]
    public void When_Order_Type_Does_Not_Match_Client_Rule_Settings_Then_Validation_Fails(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(b => b.Type);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.ClientOrderTypeMismatch);
    }

    [Theory]
    [MemberData(nameof(ClientCurrencyMismatchData))]
    [Trait("Category", "Unit")]
    public void When_Currency_Does_Not_Match_Client_Rule_Settings_Then_Validation_Fails(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(b => b.Currency);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.ClientOrderCurrencyMismatch);
    }

    [Theory]
    [MemberData(nameof(ClientDestinationMismatchData))]
    [Trait("Category", "Unit")]
    public void When_Destination_Does_Not_Match_Client_Rule_Settings_Then_Validation_Fails(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(b => b.Destination);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.ClientDestinationMismatch);
    }

    [Theory]
    [MemberData(nameof(ClientBelowMinimumChildNotionalAmountData))]
    [Trait("Category", "Unit")]
    public void When_Order_Notional_Amount_Is_Below_Client_Minimum_Then_Validation_Fails(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(b => b.NotionalAmount);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.ChildOrderNotionalAmountBelowClientsMinimum);
    }

    public static IEnumerable<object?[]> ClientOrderTypeMismatchData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o =>
            {
                o.ClientId = ClientRuleSettingsDefinition.ClientA.ClientId;
                o.Type = ClientRuleSettingsDefinition.ClientB.Type;
            }) }
        };

    public static IEnumerable<object?[]> ClientCurrencyMismatchData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o =>
            {
                o.ClientId = ClientRuleSettingsDefinition.ClientA.ClientId;
                o.Currency = ClientRuleSettingsDefinition.ClientB.Currency;
            }) }
        };

    public static IEnumerable<object?[]> ClientDestinationMismatchData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o =>
            {
                o.ClientId = ClientRuleSettingsDefinition.ClientA.ClientId;
                o.Destination = ClientRuleSettingsDefinition.ClientB.Destination;
            }) }
        };

    public static IEnumerable<object?[]> ClientBelowMinimumChildNotionalAmountData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o =>
            {
                o.ClientId = ClientRuleSettingsDefinition.ClientA.ClientId;
                o.NotionalAmount = ClientRuleSettingsDefinition.ClientA.MinimumChildNotionalAmount - 1;
            }) }
        };
}
