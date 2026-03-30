open System
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Diagnostics.Windows
open HList.Heavy

[<EntryPoint>]
let main argv =
    let isEtwMode = argv |> Array.exists  (fun s -> s.Contains("ETW", StringComparison.InvariantCulture))
#if false
        Array.windowed 2 argv 
        |> Array.map (fun ary -> (ary[0], ary[2]) )
        |> Array.exists (fun pair -> pair = ("-p", "ETW") || pair = ("--profiler", "ETW"))   
#endif

    let config : IConfig = 
        match isEtwMode with
        | true -> DefaultConfig.Instance.AddDiagnoser(EtwProfiler())
        | false -> DefaultConfig.Instance

    BenchmarkRunner.Run<Benchmarks>(config, argv) |> ignore
    0 // return an integer exit code
