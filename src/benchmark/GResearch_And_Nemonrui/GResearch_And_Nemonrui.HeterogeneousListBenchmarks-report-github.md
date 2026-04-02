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
| Method       | Job                           | Runtime              | FoldLoop | Mean      | Error     | StdDev    | Ratio | RatioSD | Code Size | Gen0      | Gen1    | Allocated | Alloc Ratio |
|------------- |------------------------------ |--------------------- |--------- |----------:|----------:|----------:|------:|--------:|----------:|----------:|--------:|----------:|------------:|
| GResearch    | ShortRun-.NET 10.0            | .NET 10.0            | 10000    |  7.459 ms | 0.7147 ms | 0.0392 ms |  1.00 |    0.01 |   2,494 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| Nemonuri_Old | ShortRun-.NET 10.0            | .NET 10.0            | 10000    |  5.619 ms | 0.9956 ms | 0.0546 ms |  0.75 |    0.01 |   1,476 B |  648.4375 |       - |   9.77 MB |        0.34 |
| Nemonuri_New | ShortRun-.NET 10.0            | .NET 10.0            | 10000    |  2.508 ms | 0.2255 ms | 0.0124 ms |  0.34 |    0.00 |   1,094 B |  324.2188 |       - |   4.88 MB |        0.17 |
|              |                               |                      |          |           |           |           |       |         |           |           |         |           |             |
| GResearch    | ShortRun-.NET 8.0             | .NET 8.0             | 10000    |  9.572 ms | 1.4982 ms | 0.0821 ms |  1.00 |    0.01 |   2,497 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| Nemonuri_Old | ShortRun-.NET 8.0             | .NET 8.0             | 10000    | 10.691 ms | 1.2416 ms | 0.0681 ms |  1.12 |    0.01 |   1,473 B |  640.6250 |       - |   9.77 MB |        0.34 |
| Nemonuri_New | ShortRun-.NET 8.0             | .NET 8.0             | 10000    |  3.336 ms | 0.3767 ms | 0.0206 ms |  0.35 |    0.00 |   1,077 B |  324.2188 |       - |   4.88 MB |        0.17 |
|              |                               |                      |          |           |           |           |       |         |           |           |         |           |             |
| GResearch    | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 11.200 ms | 0.4926 ms | 0.0270 ms |  1.00 |    0.00 |   1,075 B | 4734.3750 | 15.6250 |  28.46 MB |        1.00 |
| Nemonuri_Old | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 61.933 ms | 6.8100 ms | 0.3733 ms |  5.53 |    0.03 |     821 B | 1555.5556 |       - |   9.79 MB |        0.34 |
| Nemonuri_New | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 33.630 ms | 5.3468 ms | 0.2931 ms |  3.00 |    0.02 |   1,312 B |  800.0000 |       - |    4.9 MB |        0.17 |
