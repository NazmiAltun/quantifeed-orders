// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;
using Orders.Api.Validators;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    private readonly OrderValidator _validator = ValidatorFactory.CreateOrderValidator();

    [Fact]
    [Trait("Category", "Unit")]
    public void When_Order_Is_Valid_Then_Validation_Should_Be_Successful()
    {
        var order = OrderMother.Create();

        var result = _validator.TestValidate(order);

        result.IsValid.Should().BeTrue();
    }
}
