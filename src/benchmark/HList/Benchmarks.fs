module HList 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Diagnosers
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open BenchmarkDotNet.Configs


[<MemoryDiagnoser; ExceptionDiagnoser>]
[<AnyCategoriesFilter("Standard", "All")>]
[<ShortRunJob(RuntimeMoniker.Net10_0)>]
type Benchmarks () =

    let foldImpl (acc: int) (x: 'a) : int = if typeof<'a> = typeof<int> then acc + 1 else acc

    let hlistFolder : HListFolder<int> = 
        { new HListFolder<int> with member _.Folder (acc: int) (x: 'a): int = foldImpl acc x }

    
    let quickFolder : IFolder<int> = 
        { new IFolder<int> with member _.Step (acc: int, elem: 'T): int = foldImpl acc elem }
    
    let guardValueIs3 (n: int) : int = if n = 3 then n else failwithf "%d" n

    [<Params(1, 100)>]
    member val LoopCount = 1 with get, set


    [<Benchmark(Baseline = true)>]
    [<BenchmarkCategory("Standard", "All")>]
    member this.HList() : int =
        let l =
            HList.empty
            |> HList.cons 1
            |> HList.cons "Hello"
            |> HList.cons '.'
            |> HList.cons 3
            |> HList.cons 4.5
            |> HList.cons 0L
            |> HList.cons -7
            |> HList.cons 100u
        let mutable acc = 0
        for i = 1 to this.LoopCount do
            acc <-
            l
            |> HList.fold hlistFolder 0
            |> guardValueIs3
            |> (+) acc
        acc

    [<Benchmark>]
    [<BenchmarkCategory("Standard", "All")>]
    member this.QuickHLists() : int =
        let l =
            QuickHLists.empty
            |> QuickHLists.cons 1
            |> QuickHLists.cons "Hello"
            |> QuickHLists.cons '.'
            |> QuickHLists.cons 3
            |> QuickHLists.cons 4.5
            |> QuickHLists.cons 0L
            |> QuickHLists.cons -7
            |> QuickHLists.cons 100u
        let mutable acc = 0
        for i = 1 to this.LoopCount do
            acc <-
            l
            |> QuickHLists.fold quickFolder 0
            |> guardValueIs3
            |> (+) acc
        acc


    [<Benchmark>]
    [<BenchmarkCategory("All")>]
    member this.MinimalHList() : int =
        let l =
            MinimalHLists.empty
            |> MinimalHLists.cons 1
            |> MinimalHLists.cons "Hello"
            |> MinimalHLists.cons '.'
            |> MinimalHLists.cons 3
            |> MinimalHLists.cons 4.5
            |> MinimalHLists.cons 0L
            |> MinimalHLists.cons -7
            |> MinimalHLists.cons 100u
        let mutable acc = 0
        for i = 1 to this.LoopCount do
            acc <-
            l
            |> MinimalHLists.Folders.fold quickFolder 0
            |> guardValueIs3
            |> (+) acc
        acc
