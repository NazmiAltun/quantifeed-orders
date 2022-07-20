// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(MissingSymbolData))]
    [Trait("Category", "Unit")]
    public void When_Symbol_Is_Missing_Then_Validation_Fails_With_Symbol_Missing_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.Symbol);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.SymbolCannotBeEmpty);
    }

    public static IEnumerable<object?[]> MissingSymbolData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.Symbol = null) },
            new object?[] { OrderMother.Create(o => o.Symbol = "") }
        };
}
