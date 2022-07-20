// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public void When_Notional_Amount_Is_0_Then_NotionalAmount_Validation_Fails_With_Invalid_Notional_Amount_Error()
    {
        var order = OrderMother.Create(o => o.NotionalAmount = 0);
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.NotionalAmount);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.InvalidNotionalAmount);
    }
}
