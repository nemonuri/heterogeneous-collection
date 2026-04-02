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
| Method    | Job                           | Runtime              | Mean     | Error     | StdDev  | Ratio | RatioSD | Code Size | Gen0   | Allocated | Alloc Ratio |
|---------- |------------------------------ |--------------------- |---------:|----------:|--------:|------:|--------:|----------:|-------:|----------:|------------:|
| GResearch | ShortRun-.NET 10.0            | .NET 10.0            | 317.2 ns | 132.41 ns | 7.26 ns |  1.00 |    0.03 |   1,325 B | 0.0916 |    1440 B |        1.00 |
| Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            | 178.8 ns |   9.45 ns | 0.52 ns |  0.56 |    0.01 |     635 B | 0.0203 |     320 B |        0.22 |
|           |                               |                      |          |           |         |       |         |           |        |           |             |
| GResearch | ShortRun-.NET 8.0             | .NET 8.0             | 473.5 ns |  38.23 ns | 2.10 ns |  1.00 |    0.01 |     733 B | 0.0916 |    1440 B |        1.00 |
| Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             | 229.4 ns |  12.63 ns | 0.69 ns |  0.48 |    0.00 |     395 B | 0.0203 |     320 B |        0.22 |
|           |                               |                      |          |           |         |       |         |           |        |           |             |
| GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 508.9 ns |  18.74 ns | 1.03 ns |  1.00 |    0.00 |     726 B | 0.2289 |    1444 B |        1.00 |
| Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 309.7 ns |  24.00 ns | 1.32 ns |  0.61 |    0.00 |     605 B | 0.0505 |     321 B |        0.22 |
