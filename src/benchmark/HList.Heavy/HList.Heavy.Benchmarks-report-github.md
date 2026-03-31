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
| Method     | Job                           | Runtime              | FoldLoop | Mean      | Error     | StdDev    | Ratio | RatioSD | Code Size | Gen0      | Gen1    | Allocated | Alloc Ratio |
|----------- |------------------------------ |--------------------- |--------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|--------:|----------:|------------:|
| HList      | ShortRun-.NET 10.0            | .NET 10.0            | 10000    |  7.645 ms | 0.7802 ms | 0.0428 ms |  1.00 |    0.01 |   6,360 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| PureHLists | ShortRun-.NET 10.0            | .NET 10.0            | 10000    |  5.723 ms | 0.1539 ms | 0.0084 ms |  0.75 |    0.00 |   2,178 B |  648.4375 |       - |   9.77 MB |        0.34 |
|            |                               |                      |          |           |           |           |       |         |           |           |         |           |             |
| HList      | ShortRun-.NET 8.0             | .NET 8.0             | 10000    |  9.672 ms | 1.1894 ms | 0.0652 ms |  1.00 |    0.01 |   4,922 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| PureHLists | ShortRun-.NET 8.0             | .NET 8.0             | 10000    | 10.811 ms | 0.8321 ms | 0.0456 ms |  1.12 |    0.01 |   1,895 B |  640.6250 |       - |   9.77 MB |        0.34 |
|            |                               |                      |          |           |           |           |       |         |           |           |         |           |             |
| HList      | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 11.145 ms | 0.5613 ms | 0.0308 ms |  1.00 |    0.00 |   3,113 B | 4734.3750 | 15.6250 |  28.46 MB |        1.00 |
| PureHLists | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 62.505 ms | 4.5587 ms | 0.2499 ms |  5.61 |    0.02 |   3,114 B | 1625.0000 |       - |   9.79 MB |        0.34 |
