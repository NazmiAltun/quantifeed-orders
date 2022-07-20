// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Unit.Tests.Helpers;
using Orders.Api.Validation;

namespace Orders.Api.Unit.Tests.Validation;

public partial class OrderValidatorTests
{
    [Theory]
    [MemberData(nameof(MissingClientIdData))]
    [Trait("Category", "Unit")]
    public void When_ClientId_Is_Missing_Then_Validation_Fails_With_ClientId_Error(
        Order order)
    {
        var result = _validator.TestValidate(order);

        result.ShouldHaveValidationErrorFor(o => o.ClientId);
        result.Errors.Single().ErrorCode.Should().Be(ErrorCodes.ClientIdCannotBeEmpty);
    }

    public static IEnumerable<object?[]> MissingClientIdData =>
        new List<object?[]>
        {
            new object?[] { OrderMother.Create(o => o.ClientId = null) },
            new object?[] { OrderMother.Create(o => o.ClientId = "") }
        };
}
