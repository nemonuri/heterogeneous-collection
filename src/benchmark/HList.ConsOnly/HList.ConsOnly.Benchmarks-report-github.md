```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
12th Gen Intel Core i9-12900 2.40GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 10.0.104
  [Host]     : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3 DEBUG
  DefaultJob : .NET 10.0.4 (10.0.4, 10.0.426.12010), X64 RyuJIT x86-64-v3


```
| Method       | Mean      | Error    | StdDev   | Ratio | Exceptions | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------- |----------:|---------:|---------:|------:|-----------:|-------:|-------:|----------:|------------:|
| HList        | 117.43 ns | 1.127 ns | 0.999 ns |  1.00 |          - | 0.0632 |      - |     992 B |        1.00 |
| MinimalHList |  50.37 ns | 0.313 ns | 0.278 ns |  0.43 |          - | 0.0286 | 0.0001 |     448 B |        0.45 |
| QuickHLists  |  69.05 ns | 0.409 ns | 0.382 ns |  0.59 |          - | 0.0448 | 0.0001 |     704 B |        0.71 |
