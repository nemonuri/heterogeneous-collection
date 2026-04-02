
open BenchmarkDotNet.Running
open GResearch_And_Nemonrui

[<EntryPoint>]
let main argv =
    BenchmarkSwitcher.FromTypes([|typeof<HeterogeneousListBenchmarks>; typeof<TypeListBenchmarks>|]).Run(argv) |> ignore
    0 // return an integer exit code
