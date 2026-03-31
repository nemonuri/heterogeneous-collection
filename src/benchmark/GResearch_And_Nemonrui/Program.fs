open System
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Diagnostics.Windows
open GResearch_And_Nemonrui

[<EntryPoint>]
let main argv =
    let isEtwMode = 
#if false
        argv 
        |> Array.exists  (fun s -> s.Contains("ETW", StringComparison.InvariantCulture))
#endif
        false

    let config : IConfig = 
        match isEtwMode with
        | true -> DefaultConfig.Instance.AddDiagnoser(EtwProfiler())
        | false -> DefaultConfig.Instance

    BenchmarkRunner.Run<Benchmarks>(config, argv) |> ignore
    0 // return an integer exit code
