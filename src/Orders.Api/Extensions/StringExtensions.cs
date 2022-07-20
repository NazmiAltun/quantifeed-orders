// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace Orders.Api.Extensions;

internal static class StringExtensions
{
    public static string Format(this string template, object? arg0)
    {
        return string.Format(CultureInfo.InvariantCulture, template, arg0);
    }
}
