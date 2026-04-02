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
type TypeListBenchmarks () =

    let expected = 
        [typeof<int>; typeof<string>; typeof<bool>; typeof<char>; typeof<uint32>;
            typeof<int option>; typeof<obj>; typeof<unit>; typeof<int list>; typeof<int * string>] |> List.rev


    let guardExpected (expected: Type list) (actual: Type list) =
        List.forall2 (fun t1 t2 -> t1 = t2) expected actual
        |> function
            | true -> actual
            | false -> failwith "not expected"

    let nFolder = 
        { new IFolder<Type list> with
            member _.Step (acc: Type list, elem: 'a): Type list = acc @ [typeof<'a>] }

    member _.mkGResearch() =
        TypeList.empty
        |> TypeList.cons<int,_>
        |> TypeList.cons<string,_>
        |> TypeList.cons<bool,_>
        |> TypeList.cons<char,_>
        |> TypeList.cons<uint32,_>
        |> TypeList.cons<int option,_>
        |> TypeList.cons<obj,_>
        |> TypeList.cons<unit,_>
        |> TypeList.cons<int list,_>
        |> TypeList.cons<int * string, _>
    
    member _.mkNemonrui() =
        TypeLists.empty
        |> TypeLists.cons<int,_>
        |> TypeLists.cons<string,_>
        |> TypeLists.cons<bool,_>
        |> TypeLists.cons<char,_>
        |> TypeLists.cons<uint32,_>
        |> TypeLists.cons<int option,_>
        |> TypeLists.cons<obj,_>
        |> TypeLists.cons<unit,_>
        |> TypeLists.cons<int list,_>
        |> TypeLists.cons<int * string, _>

    [<Benchmark(Baseline = true)>]
    member this.GResearch() =
        this.mkGResearch()
        |> TypeList.toTypes
        |> guardExpected expected
    
    [<Benchmark>]
    member this.Nemonuri() =
        this.mkNemonrui()
        |> TypeLists.fold nFolder []
        |> guardExpected expected
