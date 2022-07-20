// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(InvalidCurrencyData))]
    [Trait("Category", "Unit")]
    public void When_CurrencyCode_Is_Invalid_Then_Validation_Fails_With_Invalid_Currency_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.Currency);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.InvalidCurrencyCode);
    }


    public static IEnumerable<object?[]> InvalidCurrencyData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.Currency = null)  },
            new object?[] { OrderMother.Create(o => o.Currency = "")  },
            new object?[] { OrderMother.Create(o => o.Currency = "ABC")  },
            new object?[] { OrderMother.Create(o => o.Currency = "ABCD")  }
        };
}
