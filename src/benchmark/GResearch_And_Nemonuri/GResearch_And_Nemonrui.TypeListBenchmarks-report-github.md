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
| Method    | Job                           | Runtime              | Mean     | Error    | StdDev  | Ratio | RatioSD | Gen0   | Code Size | Allocated | Alloc Ratio |
|---------- |------------------------------ |--------------------- |---------:|---------:|--------:|------:|--------:|-------:|----------:|----------:|------------:|
| GResearch | ShortRun-.NET 10.0            | .NET 10.0            | 298.8 ns | 22.46 ns | 1.23 ns |  1.00 |    0.01 | 0.0916 |   1,325 B |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            | 374.1 ns | 28.76 ns | 1.58 ns |  1.25 |    0.01 | 0.1121 |     505 B |   1.72 KB |        1.22 |
|           |                               |                      |          |          |         |       |         |        |           |           |             |
| GResearch | ShortRun-.NET 8.0             | .NET 8.0             | 443.0 ns | 14.37 ns | 0.79 ns |  1.00 |    0.00 | 0.0916 |     733 B |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             | 531.8 ns | 36.86 ns | 2.02 ns |  1.20 |    0.00 | 0.1116 |     395 B |   1.72 KB |        1.22 |
|           |                               |                      |          |          |         |       |         |        |           |           |             |
| GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 476.6 ns | 98.63 ns | 5.41 ns |  1.00 |    0.01 | 0.2289 |     726 B |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 646.7 ns | 73.73 ns | 4.04 ns |  1.36 |    0.02 | 0.2804 |     605 B |   1.72 KB |        1.22 |
