namespace GResearch_And_Nemonrui

open System
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives

module G = DiffList
module N = DiffLists

[<MemoryDiagnoser; DisassemblyDiagnoser>]
[<ShortRunJob(RuntimeMoniker.Net10_0)>]
[<ShortRunJob(RuntimeMoniker.Net80)>]
[<ShortRunJob(RuntimeMoniker.Net472)>]
type DiffListBenchMarks () =

    let gFolder =
        { new DiffListFolder<_> with
            member _.Folder state elt = state + " " + elt.ToString ()
        }
    
    let nFolder =
        { new IFolder<_> with
            member _.Step (state: _, elem: _) = state + " " + elem.ToString()
        }
    
    let expected = " 4 300 hi False"

    let guardExpected (expected: string) (actual: string) =
        match expected = actual with
        | true -> actual
        | false -> failwith $"not expected: {actual}"
    
    member _.mkGResearch() =
        let l1 = G.empty |> G.cons 300 |> G.cons 4.0 in
        let l2 = G.empty<unit> |> G.cons false |> G.cons "hi" in
        l2
        |> G.append l1
    
    member _.mkNemonuri() =
        let l1 = N.assume |> N.cons 300 |> N.cons 4.0 in
        let l2 = N.empty |> N.cons false |> N.cons "hi" in
        l2
        |> N.append l1
    
    [<Benchmark(Baseline = true)>]
    member this.GResearch() =
        this.mkGResearch()
        |> G.fold gFolder ""
        |> guardExpected expected

    [<Benchmark>]
    member this.Nemonuri() =
        this.mkNemonuri()
        |> N.fold nFolder ""
        |> guardExpected expected
