// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;
using Orders.Api.Validators;

namespace Orders.Api.Unit.Tests.Validators;

public class OrdersRequestValidatorTests
{
    private readonly OrdersRequestValidator _validator = ValidatorFactory.CreateOrdersRequestValidator();

    [Theory]
    [MemberData(nameof(EmptyOrderRequestData))]
    [Trait("Category", "Unit")]
    public void When_Request_Has_No_Orders_Then_Validation_Fails_With_OrderRequestCannotBeEmpty(OrdersRequest ordersRequest)
    {
        var result = _validator.TestValidate(ordersRequest);

        result.ShouldHaveValidationErrorFor(b => b.Orders);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.OrderRequestCannotBeEmpty);
    }

    [Theory]
    [MemberData(nameof(BasketWithRepeatingOrderIds))]
    [Trait("Category", "Unit")]
    public void When_There_Are_Repeating_OrderIds_In_The_Request_Then_Validation_Fails_With_OrderMustHaveUniqueId_Error(
        OrdersRequest ordersRequest)
    {
        var result = _validator.TestValidate(ordersRequest);

        result.ShouldHaveValidationErrorFor(b => b.Orders);
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(OrdersRequest.Orders) &&
                                                  e.ErrorCode == ErrorCodes.OrdersMustHaveUniqueId);
    }

    [Theory]
    [MemberData(nameof(NonEmptyOrderRequestData))]
    [Trait("Category", "Unit")]
    public void When_There_Are_Order_In_The_Request_Then_Validation_Successful(OrdersRequest ordersRequest)
    {
        var result = _validator.TestValidate(ordersRequest);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidBasketOrderClientIdData))]
    [Trait("Category", "Unit")]
    public void Each_Basket_Orders_Should_Be_Validated(OrdersRequest orderRequest)
    {
        var result = _validator.TestValidate(orderRequest);

        result.Errors.Should().Contain(e =>
            e.ErrorCode == ErrorCodes.ClientIdCannotBeEmpty &&
            e.PropertyName == "Orders[0].ClientId");
    }

    public static IEnumerable<object?[]> InvalidBasketOrderClientIdData =>
        new List<object?[]>
        {
            new object?[] {
                new OrdersRequest
                {
                    Orders = new[]
                    {
                        BasketOrderMother.Create(o => o.ClientId = null)
                    }
                }
            }
        };

    public static IEnumerable<object?[]> EmptyOrderRequestData =>
        new List<object?[]>
        {
            new object?[] { new OrdersRequest() },
            new object?[] { new OrdersRequest { Orders = Array.Empty<BasketOrder>()} }
        };

    public static IEnumerable<object?[]> NonEmptyOrderRequestData =>
        new List<object?[]>
        {
            new object?[] { new OrdersRequest
            {
                Orders = new[]
                {
                    BasketOrderMother.Create(),
                    BasketOrderMother.Create()
                }
            } }
        };

    public static IEnumerable<object?[]> BasketWithRepeatingOrderIds =>
        new List<object?[]>
        {
            new object?[] { new OrdersRequest
            {
                Orders = new[]
                {
                    BasketOrderMother.Create(o => o.OrderId = "ABC123"),
                    BasketOrderMother.Create(),
                    BasketOrderMother.Create(o => o.ChildOrders = new []
                    {
                        OrderMother.Create(o => o.OrderId = "ABC123")
                    })
                }
            } }
        };
}
