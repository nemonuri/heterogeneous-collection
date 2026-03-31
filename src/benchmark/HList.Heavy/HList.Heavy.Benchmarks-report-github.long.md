```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]                       : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  LongRun-.NET 10.0            : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  LongRun-.NET 8.0             : .NET 8.0.7 (8.0.7, 8.0.724.31311), X64 RyuJIT x86-64-v3
  LongRun-.NET Framework 4.7.2 : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256

IterationCount=100  LaunchCount=3  WarmupCount=15  

```
| Method     | Job                          | Runtime              | FoldLoop | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Code Size | Gen0      | Gen1    | Allocated | Alloc Ratio |
|----------- |----------------------------- |--------------------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|--------:|----------:|------------:|
| HList      | LongRun-.NET 10.0            | .NET 10.0            | 10000    | 11.389 ms | 0.9791 ms | 5.0330 ms |  7.866 ms |  1.17 |    0.68 |   6,360 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| PureHLists | LongRun-.NET 10.0            | .NET 10.0            | 10000    |  5.919 ms | 0.0276 ms | 0.1415 ms |  5.860 ms |  0.61 |    0.21 |   2,178 B |  648.4375 |       - |   9.77 MB |        0.34 |
|            |                              |                      |          |           |           |           |           |       |         |           |           |         |           |             |
| HList      | LongRun-.NET 8.0             | .NET 8.0             | 10000    | 10.172 ms | 0.0427 ms | 0.2134 ms | 10.142 ms |  1.00 |    0.03 |   4,922 B | 1890.6250 |       - |  28.38 MB |        1.00 |
| PureHLists | LongRun-.NET 8.0             | .NET 8.0             | 10000    | 11.182 ms | 0.0154 ms | 0.0791 ms | 11.187 ms |  1.10 |    0.02 |   1,895 B |  640.6250 |       - |   9.77 MB |        0.34 |
|            |                              |                      |          |           |           |           |           |       |         |           |           |         |           |             |
| HList      | LongRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 11.417 ms | 0.0079 ms | 0.0402 ms | 11.412 ms |  1.00 |    0.00 |   3,113 B | 4734.3750 | 15.6250 |  28.46 MB |        1.00 |
| PureHLists | LongRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 10000    | 64.133 ms | 0.0332 ms | 0.1692 ms | 64.115 ms |  5.62 |    0.02 |   3,114 B | 1625.0000 |       - |   9.79 MB |        0.34 |
