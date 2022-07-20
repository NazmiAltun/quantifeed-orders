// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(MissingOrderIdData))]
    [Trait("Category", "Unit")]
    public void When_OrderId_Is_Missing_Then_Validation_Fails_With_OrderId_Missing_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.OrderId);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.OrderIdCannotBeEmpty);
    }

    public static IEnumerable<object?[]> MissingOrderIdData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.OrderId = null) },
            new object?[] { OrderMother.Create(o => o.OrderId = "") }
        };
}
