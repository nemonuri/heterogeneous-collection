```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]              : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  MediumRun-.NET 10.0 : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3
  MediumRun-.NET 8.0  : .NET 8.0.7 (8.0.7, 8.0.724.31311), X64 RyuJIT x86-64-v3

IterationCount=15  LaunchCount=2  WarmupCount=10  

```
| Method     | Job                 | Runtime   | FoldLoop | Mean      | Error     | StdDev    | Ratio | Gen0      | Code Size | Allocated | Alloc Ratio |
|----------- |-------------------- |---------- |--------- |----------:|----------:|----------:|------:|----------:|----------:|----------:|------------:|
| HList      | MediumRun-.NET 10.0 | .NET 10.0 | 10000    |  7.937 ms | 0.0512 ms | 0.0718 ms |  1.00 | 1890.6250 |   6,360 B |  28.38 MB |        1.00 |
| PureHLists | MediumRun-.NET 10.0 | .NET 10.0 | 10000    |  5.932 ms | 0.0529 ms | 0.0775 ms |  0.75 |  648.4375 |   2,178 B |   9.77 MB |        0.34 |
|            |                     |           |          |           |           |           |       |           |           |           |             |
| HList      | MediumRun-.NET 8.0  | .NET 8.0  | 10000    |  9.917 ms | 0.0448 ms | 0.0656 ms |  1.00 | 1890.6250 |   4,922 B |  28.38 MB |        1.00 |
| PureHLists | MediumRun-.NET 8.0  | .NET 8.0  | 10000    | 11.292 ms | 0.0531 ms | 0.0745 ms |  1.14 |  640.6250 |   1,895 B |   9.77 MB |        0.34 |
