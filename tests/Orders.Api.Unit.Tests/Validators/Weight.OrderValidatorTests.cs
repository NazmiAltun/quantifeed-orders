// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(InvalidWeightData))]
    [Trait("Category", "Unit")]
    public void When_Weight_is_Out_Of_Range_Then_Validation_Fails_With_Weight_Is_Out_Of_Range_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.WeightIsOutOfRange);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void When_Order_Weight_Is_1_Then_Validation_Successful()
    {
        var order = OrderMother.Create(o => o.Weight = 1);
        var result = _validator.TestValidate(order);

        result.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object?[]> InvalidWeightData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.Weight = 0) },
            new object?[] { OrderMother.Create(o => o.Weight = 1.2m) }
        };
}
