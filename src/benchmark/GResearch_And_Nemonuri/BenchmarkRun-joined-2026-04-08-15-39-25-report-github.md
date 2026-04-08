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
| Type                        | Method    | Job                           | Runtime              | Mean            | Error           | StdDev        | Ratio     | RatioSD | Gen0      | Code Size | Gen1    | Allocated  | Alloc Ratio |
|---------------------------- |---------- |------------------------------ |--------------------- |----------------:|----------------:|--------------:|----------:|--------:|----------:|----------:|--------:|-----------:|------------:|
| DiffListBenchMarks          | GResearch | ShortRun-.NET 10.0            | .NET 10.0            |        220.3 ns |        21.88 ns |       1.20 ns |      1.00 |    0.01 |    0.0663 |   2,055 B |       - |     1040 B |        1.00 |
| DiffTypeListBenchMarks      | Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |        128.5 ns |        24.57 ns |       1.35 ns |      0.58 |    0.01 |    0.0162 |     730 B |       - |      256 B |        0.25 |
| HeterogeneousListBenchmarks | GResearch | ShortRun-.NET 10.0            | .NET 10.0            |  7,694,779.2 ns | 2,512,352.75 ns | 137,710.53 ns | 34,928.44 |  565.98 | 1890.6250 |   2,494 B |       - | 29760000 B |   28,615.38 |
| TypeListBenchmarks          | GResearch | ShortRun-.NET 10.0            | .NET 10.0            |        322.6 ns |        34.96 ns |       1.92 ns |      1.46 |    0.01 |    0.0916 |   1,455 B |       - |     1440 B |        1.38 |
| DiffListBenchMarks          | Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |        121.1 ns |        12.27 ns |       0.67 ns |      0.55 |    0.00 |    0.0386 |   1,251 B |       - |      608 B |        0.58 |
| HeterogeneousListBenchmarks | Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |  2,561,768.7 ns |    62,868.44 ns |   3,446.03 ns | 11,628.48 |   56.61 |  324.2188 |   1,091 B |       - |  5120000 B |    4,923.08 |
| TypeListBenchmarks          | Nemonuri  | ShortRun-.NET 10.0            | .NET 10.0            |        176.8 ns |        26.44 ns |       1.45 ns |      0.80 |    0.01 |    0.0203 |     538 B |       - |      320 B |        0.31 |
|                             |           |                               |                      |                 |                 |               |           |         |           |           |         |            |             |
| DiffListBenchMarks          | GResearch | ShortRun-.NET 8.0             | .NET 8.0             |        306.5 ns |        23.73 ns |       1.30 ns |      1.00 |    0.01 |    0.0663 |   1,492 B |       - |     1040 B |        1.00 |
| HeterogeneousListBenchmarks | GResearch | ShortRun-.NET 8.0             | .NET 8.0             |  9,590,845.3 ns | 1,703,545.41 ns |  93,377.07 ns | 31,289.01 |  287.68 | 1890.6250 |   2,497 B |       - | 29760000 B |   28,615.38 |
| TypeListBenchmarks          | GResearch | ShortRun-.NET 8.0             | .NET 8.0             |        455.6 ns |        43.06 ns |       2.36 ns |      1.49 |    0.01 |    0.0916 |     733 B |       - |     1440 B |        1.38 |
| DiffListBenchMarks          | Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             |              NA |              NA |            NA |         ? |       ? |        NA |        NA |      NA |         NA |           ? |
| HeterogeneousListBenchmarks | Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             |              NA |              NA |            NA |         ? |       ? |        NA |        NA |      NA |         NA |           ? |
| TypeListBenchmarks          | Nemonuri  | ShortRun-.NET 8.0             | .NET 8.0             |              NA |              NA |            NA |         ? |       ? |        NA |        NA |      NA |         NA |           ? |
|                             |           |                               |                      |                 |                 |               |           |         |           |           |         |            |             |
| DiffListBenchMarks          | GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |        458.7 ns |        33.31 ns |       1.83 ns |      1.00 |    0.00 |    0.1683 |   1,437 B |       - |     1059 B |        1.00 |
| DiffTypeListBenchMarks      | Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |        295.3 ns |        27.96 ns |       1.53 ns |      0.64 |    0.00 |    0.0405 |     660 B |       - |      257 B |        0.24 |
| HeterogeneousListBenchmarks | GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 11,094,495.8 ns | 2,584,264.06 ns | 141,652.23 ns | 24,186.38 |  280.11 | 4734.3750 |   1,075 B | 15.6250 | 29847555 B |   28,184.66 |
| TypeListBenchmarks          | GResearch | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |        498.3 ns |        29.91 ns |       1.64 ns |      1.09 |    0.00 |    0.2289 |     726 B |       - |     1444 B |        1.36 |
| DiffListBenchMarks          | Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |      1,063.6 ns |       160.22 ns |       8.78 ns |      2.32 |    0.02 |    0.0992 |   1,143 B |       - |      626 B |        0.59 |
| HeterogeneousListBenchmarks | Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 | 33,499,784.4 ns | 4,040,540.60 ns | 221,475.67 ns | 73,030.66 |  487.94 |  800.0000 |   1,314 B |       - |  5134790 B |    4,848.72 |
| TypeListBenchmarks          | Nemonuri  | ShortRun-.NET Framework 4.7.2 | .NET Framework 4.7.2 |        348.0 ns |        27.03 ns |       1.48 ns |      0.76 |    0.00 |    0.0505 |     435 B |       - |      321 B |        0.30 |

Benchmarks with issues:
  DiffListBenchMarks.Nemonuri: ShortRun-.NET 8.0(Runtime=.NET 8.0, IterationCount=3, LaunchCount=1, WarmupCount=3)
  HeterogeneousListBenchmarks.Nemonuri: ShortRun-.NET 8.0(Runtime=.NET 8.0, IterationCount=3, LaunchCount=1, WarmupCount=3)
  TypeListBenchmarks.Nemonuri: ShortRun-.NET 8.0(Runtime=.NET 8.0, IterationCount=3, LaunchCount=1, WarmupCount=3)
