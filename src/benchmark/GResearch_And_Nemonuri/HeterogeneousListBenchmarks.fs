namespace GResearch_And_Nemonrui

open System
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives


[<MemoryDiagnoser; DisassemblyDiagnoser>]
[<ShortRunJob(RuntimeMoniker.Net10_0)>]
[<ShortRunJob(RuntimeMoniker.Net80)>]
[<ShortRunJob(RuntimeMoniker.Net472)>]
type HeterogeneousListBenchmarks () =

    let foldImpl (acc: int) (x: 'a) : int = if typeof<'a> = typeof<int> then acc + 1 else acc

    let gFolder : HListFolder<int> = 
        { new HListFolder<int> with member _.Folder (acc: int) (x: 'a): int = foldImpl acc x }

    
    let nFolder : IFolder<int> = 
        { new IFolder<int> with member _.Step (acc: int, elem: 'T): int = foldImpl acc elem }

    let foldRefImpl (acc: inref<int>) (x: inref<'a>) : int = if typeof<'a> = typeof<int> then acc + 1 else acc
    
    let guardValueIs5 (n: int) : int = if n = 5 then n else failwithf "%d" n

    static let anonRecord = {| Name = "John"; Age = 23 |}

    // [<Params(1, 100, 10000)>]
    member val public FoldLoop:int = 10000 with get,set

    member _.mkGResearch() =
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

    member _.mkNemonuri() =
        HeterogeneousLists.empty
        |> HeterogeneousLists.cons 1
        |> HeterogeneousLists.cons "Hello"
        |> HeterogeneousLists.cons '.'
        |> HeterogeneousLists.cons 3
        |> HeterogeneousLists.cons 4.5
        |> HeterogeneousLists.cons 0L
        |> HeterogeneousLists.cons -7
        |> HeterogeneousLists.cons 100u
        |> HeterogeneousLists.cons struct (1, 2)
        |> HeterogeneousLists.cons '5'B
        |> HeterogeneousLists.cons ()
        |> HeterogeneousLists.cons 8
        |> HeterogeneousLists.cons DBNull.Value
        |> HeterogeneousLists.cons 0
        |> HeterogeneousLists.cons anonRecord
        |> HeterogeneousLists.cons (ValueSome 3)


    [<Benchmark(Baseline = true)>]
    member this.GResearch() : int =
        let mutable acc: int = 0
        for i = 1 to this.FoldLoop do
            acc <-
            this.mkGResearch()
            |> HList.fold gFolder 0
            |> guardValueIs5
        acc

    [<Benchmark>]
    member this.Nemonuri() : int =
        let mutable acc: int = 0
        for i = 1 to this.FoldLoop do
            acc <-
            this.mkNemonuri()
            |> HeterogeneousLists.fold nFolder 0
            |> guardValueIs5
        acc
