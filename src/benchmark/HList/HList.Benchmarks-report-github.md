```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]     : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  DefaultJob : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3


```
| Method       | LoopCount | Mean        | Error     | StdDev    | Ratio | Exceptions | Gen0   | Allocated | Alloc Ratio |
|------------- |---------- |------------:|----------:|----------:|------:|-----------:|-------:|----------:|------------:|
| **HList**        | **1**         |    **356.0 ns** |   **2.95 ns** |   **2.76 ns** |  **1.00** |          **-** | **0.0958** |    **1504 B** |        **1.00** |
| MinimalHList | 1         |    152.5 ns |   1.00 ns |   0.94 ns |  0.43 |          - | 0.0284 |     448 B |        0.30 |
| QuickHLists  | 1         |    351.5 ns |   2.57 ns |   2.40 ns |  0.99 |          - | 0.0448 |     704 B |        0.47 |
|              |           |             |           |           |       |            |        |           |             |
| **HList**        | **100**       | **22,288.1 ns** | **219.01 ns** | **204.86 ns** |  **1.00** |          **-** | **3.3264** |   **52192 B** |       **1.000** |
| MinimalHList | 100       | 12,218.0 ns |  38.24 ns |  33.90 ns |  0.55 |          - | 0.0153 |     448 B |       0.009 |
| QuickHLists  | 100       | 26,754.9 ns |  95.73 ns |  89.54 ns |  1.20 |          - | 0.0305 |     704 B |       0.013 |
