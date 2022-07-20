// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Repositories;

public record SaveResult(
    bool IsSuccessful,
    List<string> ExistingOrderIds);
