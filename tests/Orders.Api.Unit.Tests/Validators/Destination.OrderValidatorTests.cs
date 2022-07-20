// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;

namespace Orders.Api.Unit.Tests.Validators;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(MissingDestinationData))]
    [Trait("Category", "Unit")]
    public void When_Destination_Is_Missing_Then_Validation_Fails_With_Destination_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.Destination);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.DestinationCannotBeEmpty);
    }

    public static IEnumerable<object?[]> MissingDestinationData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.Destination = null) },
            new object?[] { OrderMother.Create(o => o.Destination = "") }
        };
}
