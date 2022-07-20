// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;
using Orders.Api.Validators;

namespace Orders.Api.Unit.Tests.Validators;

public class BasketOrderValidatorTests
{
    private readonly BasketOrderValidator _validator = ValidatorFactory.CreateBasketOrderValidator();

    [Theory]
    [MemberData(nameof(MissingChildOrderData))]
    [Trait("Category", "Unit")]
    public void When_Basket_Order_Has_No_Child_Orders_Then_Validation_Fails(
        BasketOrder basketOrder)
    {
        var result = _validator.TestValidate(basketOrder);

        result.ShouldHaveValidationErrorFor(o => o.ChildOrders);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.BasketOrderMustHaveChildOrders);
    }

    [Theory]
    [MemberData(nameof(ChildOrderDataWithInvalidWeights))]
    [Trait("Category", "Unit")]
    public void When_Basket_Order_Child_Orders_Weight_Sum_Not_1_Then_Validation_Fails(
        BasketOrder basketOrder)
    {
        var result = _validator.TestValidate(basketOrder);

        result.ShouldHaveValidationErrorFor(o => o.Weight);
        result.Errors.Should().Contain(e => e.ErrorCode == ErrorCodes.InvalidChildOrderWeightSum &&
                                            e.PropertyName == nameof(BasketOrder.Weight));
    }

    [Theory]
    [MemberData(nameof(ClientBelowMinimumBasketNotionalAmountData))]
    [Trait("Category", "Unit")]
    public void When_Basket_Notional_Amount_Is_Below_Client_Minimum_Then_Validation_Fails(
        BasketOrder order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(b => b.NotionalAmount);
        result.Errors.Should().Contain(e => e.PropertyName == nameof(BasketOrder.NotionalAmount) &&
            e.ErrorCode == ErrorCodes.BasketNotionalAmountBelowClientsMinimum);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void When_Basket_Order_Is_Valid_Then_Validation_Successful()
    {
        var basketOrder = BasketOrderMother.Create();

        var result = _validator.TestValidate(basketOrder);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void Each_Child_Orders_Should_Be_Validated()
    {
        var basketOrder = BasketOrderMother.Create(o =>
        {
            o.ChildOrders!.First().ClientId = null;
        });

        var result = _validator.TestValidate(basketOrder);

        result.Errors.Should().Contain(e =>
            e.ErrorCode == ErrorCodes.ClientIdCannotBeEmpty &&
            e.PropertyName == "ChildOrders[0].ClientId");
    }

    public static IEnumerable<object?[]> MissingChildOrderData =>
        new List<object?[]>
        {
            new object?[] { BasketOrderMother.Create(o => o.ChildOrders = null) },
            new object?[] { BasketOrderMother.Create(o => o.ChildOrders = new List<Order>()) }
        };

    public static IEnumerable<object?[]> ChildOrderDataWithInvalidWeights =>
        new List<object?[]>
        {
            new object?[] { BasketOrderMother.Create(o => o.ChildOrders = new List<Order>()
            {
                OrderMother.Create(o => o.Weight = 0.5m),
                OrderMother.Create(o => o.Weight = 0.45m)
            }) },
            new object?[] { BasketOrderMother.Create(o => o.ChildOrders = new List<Order>()
            {
                OrderMother.Create(o => o.Weight = 0.75m),
                OrderMother.Create(o => o.Weight = 0.35m)
            }) }
        };

    public static IEnumerable<object?[]> ClientBelowMinimumBasketNotionalAmountData =>
        new List<object?[]>
        {
            new object?[] { BasketOrderMother.Create(o =>
            {
                o.ClientId = ClientRuleSettingsDefinition.ClientB.ClientId;
                o.ChildOrders = new List<Order>
                {
                    OrderMother.Create(o => o.NotionalAmount = 1),
                    OrderMother.Create(o => o.NotionalAmount = 2),
                    OrderMother.Create(o => o.NotionalAmount = 3)
                };
            }) }
        };
}
