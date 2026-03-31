module HList.Heavy 

open System
open BenchmarkDotNet
open BenchmarkDotNet
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open BenchmarkDotNet.Diagnostics.Windows.Configs

//[<EtwProfiler>]
[<MemoryDiagnoser>]
[<DisassemblyDiagnoser(maxDepth = 2, exportGithubMarkdown = false, exportHtml = true, exportCombinedDisassemblyReport = true)>]
[<ShortRunJob(RuntimeMoniker.Net10_0)>]
[<ShortRunJob(RuntimeMoniker.Net80)>]
[<ShortRunJob(RuntimeMoniker.Net472)>]
type Benchmarks () =

    let foldImpl (acc: int) (x: 'a) : int = if typeof<'a> = typeof<int> then acc + 1 else acc

    let hlistFolder : HListFolder<int> = 
        { new HListFolder<int> with member _.Folder (acc: int) (x: 'a): int = foldImpl acc x }

    
    let quickFolder : IFolder<int> = 
        { new IFolder<int> with member _.Step (acc: int, elem: 'T): int = foldImpl acc elem }

    let foldRefImpl (acc: inref<int>) (x: inref<'a>) : int = if typeof<'a> = typeof<int> then acc + 1 else acc
    
    let guardValueIs5 (n: int) : int = if n = 5 then n else failwithf "%d" n

    static let anonRecord = {| Name = "John"; Age = 23 |}

    [<Params((*1, 100,*) 10000)>]
    member val public FoldLoop:int = 1 with get,set

    member _.mkHList() =
        HList.empty
        |> HList.cons 1
        |> HList.cons "Hello"
        |> HList.cons '.'
        |> HList.cons 3
        |> HList.cons 4.5
        |> HList.cons 0L
        |> HList.cons -7
        |> HList.cons 100u
        |> HList.cons struct (1, 2)
        |> HList.cons '5'B
        |> HList.cons ()
        |> HList.cons 8
        |> HList.cons DBNull.Value
        |> HList.cons 0
        |> HList.cons anonRecord
        |> HList.cons (ValueSome 3)

    member _.mkPureHList() =
        PureHLists.empty
        |> PureHLists.cons 1
        |> PureHLists.cons "Hello"
        |> PureHLists.cons '.'
        |> PureHLists.cons 3
        |> PureHLists.cons 4.5
        |> PureHLists.cons 0L
        |> PureHLists.cons -7
        |> PureHLists.cons 100u
        |> PureHLists.cons struct (1, 2)
        |> PureHLists.cons '5'B
        |> PureHLists.cons ()
        |> PureHLists.cons 8
        |> PureHLists.cons DBNull.Value
        |> PureHLists.cons 0
        |> PureHLists.cons anonRecord
        |> PureHLists.cons (ValueSome 3)

    [<Benchmark(Baseline = true)>]
    member this.HList() : int =
        let mutable acc: int = 0
        for i = 1 to this.FoldLoop do
            acc <-
            this.mkHList()
            |> HList.fold hlistFolder 0
            |> guardValueIs5
        acc

    [<Benchmark>]
    member this.PureHLists() : int =
        let mutable acc: int = 0
        for i = 1 to this.FoldLoop do
            acc <-
            this.mkPureHList()
            |> PureHLists.fold quickFolder 0
            |> guardValueIs5
        acc