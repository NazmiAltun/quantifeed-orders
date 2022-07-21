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
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

        var config = configuration.Build();
        Url = config["GrpcApiUrl"];
        StartRequestCount = int.Parse(config["StartRequestCount"], CultureInfo.InvariantCulture);
        RampUpRequestCount = int.Parse(config["RampUpRequestCount"], CultureInfo.InvariantCulture);
        TotalRequestCount = int.Parse(config["TotalRequestCount"], CultureInfo.InvariantCulture);
        InvalidRequestPercent = int.Parse(config["InvalidRequestPercent"], CultureInfo.InvariantCulture);
        ExistingIdRequestPercent = int.Parse(config["ExistingIdRequestPercent"], CultureInfo.InvariantCulture);

        Console.WriteLine("Settings");
        Console.WriteLine("========");
        Console.WriteLine($"Url: {Url}");
        Console.WriteLine($"StartRequestCount: {StartRequestCount}");
        Console.WriteLine($"RampUpRequestCount: {RampUpRequestCount}");
        Console.WriteLine($"TotalRequestCount: {TotalRequestCount}");
        Console.WriteLine($"InvalidRequestPercent: {InvalidRequestPercent}");
        Console.WriteLine($"ExistingIdRequestPercent: {ExistingIdRequestPercent}");
    }

    public static string Url { get; }
    public static int StartRequestCount { get; }
    public static int RampUpRequestCount { get; }
    public static long TotalRequestCount { get; }
    public static int InvalidRequestPercent { get; }
    public static int ExistingIdRequestPercent { get; }
}
