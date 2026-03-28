module HList 

open System
open BenchmarkDotNet
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives


[<MemoryDiagnoser; ExceptionDiagnoser>]
type Benchmarks () =

    let foldImpl (acc: int) (x: 'a) : int = if typeof<'a> = typeof<int> then acc + 1 else acc

    let hlistFolder : HListFolder<int> = 
        { new HListFolder<int> with member _.Folder (acc: int) (x: 'a): int = foldImpl acc x }
    
    let quickFolder : IFolder<int> = 
        { new IFolder<int> with member _.Step (acc: int, elem: 'T): int = foldImpl acc elem }
    
    let guardValueIs3 (n: int) : int = if n = 3 then n else failwithf "%d" n

    [<Benchmark(Baseline = true)>]
    member _.HList() : int =
        HList.empty
        |> HList.cons 1
        |> HList.cons "Hello"
        |> HList.cons '.'
        |> HList.cons 3
        |> HList.cons 4.5
        |> HList.cons 0L
        |> HList.cons -7
        |> HList.cons 100u
        |> HList.fold hlistFolder 0
        |> guardValueIs3
    
    [<Benchmark>]
    member _.MinimalHList() : int =
        MinimalHLists.empty
        |> MinimalHLists.cons 1
        |> MinimalHLists.cons "Hello"
        |> MinimalHLists.cons '.'
        |> MinimalHLists.cons 3
        |> MinimalHLists.cons 4.5
        |> MinimalHLists.cons 0L
        |> MinimalHLists.cons -7
        |> MinimalHLists.cons 100u
        |> MinimalHLists.Folders.fold quickFolder 0
        |> guardValueIs3

    [<Benchmark>]
    member _.QuickHLists() : int =
        QuickHLists.empty
        |> QuickHLists.cons 1
        |> QuickHLists.cons "Hello"
        |> QuickHLists.cons '.'
        |> QuickHLists.cons 3
        |> QuickHLists.cons 4.5
        |> QuickHLists.cons 0L
        |> QuickHLists.cons -7
        |> QuickHLists.cons 100u
        |> QuickHLists.fold quickFolder 0
        |> guardValueIs3



