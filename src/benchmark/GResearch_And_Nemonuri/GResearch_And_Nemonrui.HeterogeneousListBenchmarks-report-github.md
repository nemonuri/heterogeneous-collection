```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]                        : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  ShortRun-.NET 10.0            : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  ShortRun-.NET 8.0             : .NET 8.0.7 (8.0.7, 8.0.724.31311), X64 RyuJIT x86-64-v3
  ShortRun-.NET Framework 4.7.2 : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256

IterationCount=3  LaunchCount=1  WarmupCount=3  

```
| Method    | Job                           | Runtime              | Mean      | Error     | StdDev    | Ratio | RatioSD | Code Size | Gen0      | Gen1    | Allocated | Alloc Ratio |
|---------- |------------------------------ |--------------------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|--------:|----------:|------------:|
| GResearch | ShortRun-.NET 10.0            | .NET 10.0            |  7.755 ms | 0.2502 ms | 0.0137 ms |  1.00 |    0.00 |   2,494 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |  2.638 ms | 0.2478 ms | 0.0136 ms |  0.34 |    0.00 |   1,094 B |  324.2188 |       - |   4.88 MB |        0.17 |
|           |                               |                      |           |           |           |       |         |           |           |         |           |             |
| GResearch | ShortRun-.NET 8.0             | .NET 8.0             |  9.673 ms | 1.2413 ms | 0.0680 ms |  1.00 |    0.01 |   2,497 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             |  3.374 ms | 0.2529 ms | 0.0139 ms |  0.35 |    0.00 |   1,077 B |  324.2188 |       - |   4.88 MB |        0.17 |
|           |                               |                      |           |           |           |       |         |           |           |         |           |             |
| GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 11.182 ms | 0.4742 ms | 0.0260 ms |  1.00 |    0.00 |   1,075 B | 4734.3750 | 15.6250 |  28.46 MB |        1.00 |
| Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 34.722 ms | 3.5110 ms | 0.1925 ms |  3.11 |    0.02 |   1,312 B |  800.0000 |       - |    4.9 MB |        0.17 |
