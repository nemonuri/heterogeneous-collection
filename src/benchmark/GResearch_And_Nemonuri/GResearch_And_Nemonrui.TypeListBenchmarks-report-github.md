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
| Method    | Job                           | Runtime              | Mean     | Error     | StdDev  | Ratio | Code Size | Gen0   | Allocated | Alloc Ratio |
|---------- |------------------------------ |--------------------- |---------:|----------:|--------:|------:|----------:|-------:|----------:|------------:|
| GResearch | ShortRun-.NET 10.0            | .NET 10.0            | 307.8 ns |  59.47 ns | 3.26 ns |  1.00 |   1,325 B | 0.0916 |    1440 B |        1.00 |
| Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            | 173.1 ns |  38.99 ns | 2.14 ns |  0.56 |     635 B | 0.0203 |     320 B |        0.22 |
|           |                               |                      |          |           |         |       |           |        |           |             |
| GResearch | ShortRun-.NET 8.0             | .NET 8.0             | 449.0 ns |  78.21 ns | 4.29 ns |  1.00 |     733 B | 0.0916 |    1440 B |        1.00 |
| Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             | 217.9 ns |  30.30 ns | 1.66 ns |  0.49 |     395 B | 0.0203 |     320 B |        0.22 |
|           |                               |                      |          |           |         |       |           |        |           |             |
| GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 489.2 ns | 100.75 ns | 5.52 ns |  1.00 |     726 B | 0.2289 |    1444 B |        1.00 |
| Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 289.6 ns |  40.52 ns | 2.22 ns |  0.59 |     605 B | 0.0505 |     321 B |        0.22 |
