// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Orders.Api.Stress.Test;

public static class Configuration
{
    static Configuration()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json");

        var config = configuration.Build();
        Url = config["GrpcApiUrl"];
        InvalidRequestPercent = int.Parse(config["InvalidRequestPercent"], CultureInfo.InvariantCulture);
        ExistingOrderIdRequestPercent = int.Parse(config["ExistingOrderIdRequestPercent"], CultureInfo.InvariantCulture);
        StartRequestCount = int.Parse(config["StartRequestCount"], CultureInfo.InvariantCulture);
        RampUpRequestCount = int.Parse(config["RampUpRequestCount"], CultureInfo.InvariantCulture);
        TotalRequestCount = int.Parse(config["TotalRequestCount"], CultureInfo.InvariantCulture);
    }

    public static string Url { get; }
    // e.g; when it's  3%, then every 3 out of 100 requests will be invalid. Server will return validation errors before persisting
    public static int InvalidRequestPercent { get; }
    // Every X out of 100 requests will contain order ids that already exist
    public static int ExistingOrderIdRequestPercent { get; }
    public static int StartRequestCount { get; }
    public static int RampUpRequestCount { get; }
    public static long TotalRequestCount { get; }
}
