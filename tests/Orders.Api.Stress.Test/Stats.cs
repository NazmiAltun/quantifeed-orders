// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Orders.Api.Stress.Test;

public static class Stats
{
    private static long _totalSentCount;
    private static long _totalSuccessCount;
    private static long _totalFailCount;
    private static int _requestPerSecond;
    private static readonly Stopwatch Stopwatch = new();

    public static long TotalSentCount => _totalSentCount;
    public static long TotalSuccessCount => _totalSuccessCount;
    public static long TotalFailCount => _totalFailCount;

    public static void IncrementSuccess()
    {
        Interlocked.Increment(ref _totalSuccessCount);
        IncrementTotal();
    }

    public static void IncrementFail()
    {
        Interlocked.Increment(ref _totalFailCount);
        IncrementTotal();
    }

    private static void IncrementTotal()
    {
        Interlocked.Increment(ref _totalSentCount);
    }

    public static void StartStep()
    {
        Stopwatch.Reset();
        Stopwatch.Start();
    }

    public static void EndStep(int stepSize)
    {
        Stopwatch.Stop();
        Interlocked.Exchange(ref _requestPerSecond, (int)(stepSize / Stopwatch.Elapsed.TotalSeconds));
    }

    public static void Display()
    {
        Console.WriteLine($"Rps:{_requestPerSecond}.Total Req: {TotalSentCount} Success:{TotalSuccessCount} Fail:{TotalFailCount}");
    }
}
