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

[<MemoryDiagnoser>]
type Benchmarks () =

    let foldImpl (acc: int) (x: 'a) : int = if typeof<'a> = typeof<int> then acc + 1 else acc

    let hlistFolder : HListFolder<int> = 
        { new HListFolder<int> with member _.Folder (acc: int) (x: 'a): int = foldImpl acc x }

    
    let quickFolder : IFolder<int> = 
        { new IFolder<int> with member _.Step (acc: int, elem: 'T): int = foldImpl acc elem }
    
    let guardValueIs5 (n: int) : int = if n = 5 then n else failwithf "%d" n

    let anonRecord = {| Name = "John"; Age = 23 |}

    [<Params(1, 100, 10000)>]
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

    member _.mkQuickHList() =
        QuickHLists.empty
        |> QuickHLists.cons 1
        |> QuickHLists.cons "Hello"
        |> QuickHLists.cons '.'
        |> QuickHLists.cons 3
        |> QuickHLists.cons 4.5
        |> QuickHLists.cons 0L
        |> QuickHLists.cons -7
        |> QuickHLists.cons 100u
        |> QuickHLists.cons struct (1, 2)
        |> QuickHLists.cons '5'B
        |> QuickHLists.cons ()
        |> QuickHLists.cons 8
        |> QuickHLists.cons DBNull.Value
        |> QuickHLists.cons 0
        |> QuickHLists.cons anonRecord
        |> QuickHLists.cons (ValueSome 3)

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
    member this.QuickHLists() : int =
        let mutable acc: int = 0
        for i = 1 to this.FoldLoop do
            acc <-
            this.mkQuickHList()
            |> QuickHLists.fold quickFolder 0
            |> guardValueIs5
        acc