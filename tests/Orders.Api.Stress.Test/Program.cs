// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Orders.Api.Stress.Test;

Console.WriteLine($"Waiting 5 seconds for API to warm up...Host: {Configuration.Url}");
await Task.Delay(5000);
Console.WriteLine($"Stressing started on host {Configuration.Url}");

await Orchestrator.StressAsync();
Console.WriteLine("Stressing completed...");
