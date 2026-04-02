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
| Method    | Job                           | Runtime              | Mean       | Error     | StdDev  | Ratio | RatioSD | Code Size | Gen0   | Allocated | Alloc Ratio |
|---------- |------------------------------ |--------------------- |-----------:|----------:|--------:|------:|--------:|----------:|-------:|----------:|------------:|
| GResearch | ShortRun-.NET 10.0            | .NET 10.0            |   322.6 ns |  27.96 ns | 1.53 ns |  1.00 |    0.01 |   1,325 B | 0.0916 |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |   753.7 ns |  56.36 ns | 3.09 ns |  2.34 |    0.01 |   1,889 B | 0.1068 |   1.65 KB |        1.17 |
|           |                               |                      |            |           |         |       |         |           |        |           |             |
| GResearch | ShortRun-.NET 8.0             | .NET 8.0             |   459.4 ns |  58.21 ns | 3.19 ns |  1.00 |    0.01 |     733 B | 0.0916 |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             |   898.4 ns | 101.24 ns | 5.55 ns |  1.96 |    0.02 |     665 B | 0.1192 |   1.84 KB |        1.31 |
|           |                               |                      |            |           |         |       |         |           |        |           |             |
| GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |   493.3 ns |  22.53 ns | 1.24 ns |  1.00 |    0.00 |     726 B | 0.2289 |   1.41 KB |        1.00 |
| Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 2,162.8 ns |  87.88 ns | 4.82 ns |  4.38 |    0.01 |     630 B | 0.2975 |   1.84 KB |        1.31 |
