using Orders.Api.Stress.Test;

Console.WriteLine($"Stressing started on host {Configuration.Url}");

await Orchestrator.StressAsync();
Console.WriteLine("Stressing completed...");
