```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]     : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  DefaultJob : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3


```
| Method       | Mean     | Error   | StdDev  | Ratio | Exceptions | Gen0   | Allocated | Alloc Ratio |
|------------- |---------:|--------:|--------:|------:|-----------:|-------:|----------:|------------:|
| HList        | 344.9 ns | 2.36 ns | 2.09 ns |  1.00 |          - | 0.0958 |    1504 B |        1.00 |
| MinimalHList | 152.3 ns | 1.11 ns | 0.99 ns |  0.44 |          - | 0.0284 |     448 B |        0.30 |
| QuickHLists  | 451.9 ns | 3.38 ns | 3.16 ns |  1.31 |          - | 0.0448 |     704 B |        0.47 |
