```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]        : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  .NET 10.0     : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  .NET 8.0      : .NET 8.0.7 (8.0.7, 8.0.724.31311), X64 RyuJIT x86-64-v3
  NativeAOT 8.0 : .NET 8.0.25, X64 NativeAOT x86-64-v3


```
| Method      | Job           | Runtime       | LoopCount | Mean        | Error     | StdDev    | Ratio | Gen0   | Exceptions | Allocated | Alloc Ratio |
|------------ |-------------- |-------------- |---------- |------------:|----------:|----------:|------:|-------:|-----------:|----------:|------------:|
| **HList**       | **.NET 10.0**     | **.NET 10.0**     | **1**         |    **348.5 ns** |   **1.09 ns** |   **1.02 ns** |  **1.00** | **0.0958** |          **-** |    **1504 B** |        **1.00** |
| QuickHLists | .NET 10.0     | .NET 10.0     | 1         |    365.6 ns |   1.13 ns |   1.06 ns |  1.05 | 0.0448 |          - |     704 B |        0.47 |
|             |               |               |           |             |           |           |       |        |            |           |             |
| HList       | .NET 8.0      | .NET 8.0      | 1         |    445.8 ns |   1.13 ns |   1.06 ns |  1.00 | 0.0958 |          - |    1504 B |        1.00 |
| QuickHLists | .NET 8.0      | .NET 8.0      | 1         |    731.4 ns |   1.31 ns |   1.16 ns |  1.64 | 0.0448 |          - |     704 B |        0.47 |
|             |               |               |           |             |           |           |       |        |            |           |             |
| HList       | NativeAOT 8.0 | NativeAOT 8.0 | 1         |    390.5 ns |   0.92 ns |   0.86 ns |  1.00 | 0.0958 |          - |    1504 B |        1.00 |
| QuickHLists | NativeAOT 8.0 | NativeAOT 8.0 | 1         |    432.7 ns |   0.91 ns |   0.85 ns |  1.11 | 0.0448 |          - |     704 B |        0.47 |
|             |               |               |           |             |           |           |       |        |            |           |             |
| **HList**       | **.NET 10.0**     | **.NET 10.0**     | **100**       | **21,578.7 ns** |  **76.89 ns** |  **71.92 ns** |  **1.00** | **3.3264** |          **-** |   **52192 B** |        **1.00** |
| QuickHLists | .NET 10.0     | .NET 10.0     | 100       | 26,354.7 ns |  30.16 ns |  28.21 ns |  1.22 | 0.0305 |          - |     704 B |        0.01 |
|             |               |               |           |             |           |           |       |        |            |           |             |
| HList       | .NET 8.0      | .NET 8.0      | 100       | 30,583.7 ns |  78.32 ns |  73.26 ns |  1.00 | 3.3264 |          - |   52192 B |        1.00 |
| QuickHLists | .NET 8.0      | .NET 8.0      | 100       | 61,849.4 ns | 156.57 ns | 138.79 ns |  2.02 |      - |          - |     704 B |        0.01 |
|             |               |               |           |             |           |           |       |        |            |           |             |
| HList       | NativeAOT 8.0 | NativeAOT 8.0 | 100       | 24,085.5 ns |  37.64 ns |  35.21 ns |  1.00 | 3.3264 |          - |   52192 B |        1.00 |
| QuickHLists | NativeAOT 8.0 | NativeAOT 8.0 | 100       | 23,511.3 ns |  58.68 ns |  52.02 ns |  0.98 | 0.0305 |          - |     704 B |        0.01 |
