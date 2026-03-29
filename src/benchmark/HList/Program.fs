open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Diagnostics.Windows;
open HList

type Dummy = struct end

[<EntryPoint>]
let main argv =
#if false
    BenchmarkSwitcher.FromAssembly(typeof<Dummy>.Assembly)
    |> _.Run(Array.append argv [|"--profiler"; "ETW"|], DefaultConfig.Instance.AddDiagnoser(new EtwProfiler()))
    |> ignore
#endif
    BenchmarkRunner.Run<Benchmarks>() |> ignore
    0 // return an integer exit code