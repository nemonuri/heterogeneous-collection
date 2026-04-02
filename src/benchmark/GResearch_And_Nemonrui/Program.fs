open System
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Diagnostics.Windows
open GResearch_And_Nemonrui

[<EntryPoint>]
let main argv =
    BenchmarkRunner.Run<HeterogeneousListBenchmarks>() |> ignore
    //BenchmarkRunner.Run<TypeListBenchmarks>() |> ignore
    0 // return an integer exit code
