// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Orders.Api.Extensions;

internal static class OrdersWebApplicationBuilderExtensions
{
    public static OrdersApiSettings LoadSettings(this WebApplicationBuilder webApplicationBuilder)
    {
        var settings = new OrdersApiSettings();
        webApplicationBuilder.Configuration.Bind(settings);
        ValidateSettings(settings);

        return settings;
    }

    private static void ValidateSettings(OrdersApiSettings settings)
    {
        if (settings.BasketOrderChildSumWeight == 0 ||
            settings.ClientRuleSettings?.Length == 0)
        {
            throw new InvalidOperationException("Settings are missing or failed to read.");
        }
    }
}
