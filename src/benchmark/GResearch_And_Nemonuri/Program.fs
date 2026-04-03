
open BenchmarkDotNet.Running
open GResearch_And_Nemonrui

let runBench (argv: string array) (bs: BenchmarkSwitcher) = 
    if Array.isEmpty argv then
        bs.RunAllJoined() |> ignore
    else
        bs.Run(argv) |> ignore

[<EntryPoint>]
let main argv =
    BenchmarkSwitcher.FromTypes([|
        typeof<HeterogeneousListBenchmarks>; 
        typeof<TypeListBenchmarks>;
        typeof<DiffTypeListBenchMarks>|])
    |> runBench argv
    0 // return an integer exit code
