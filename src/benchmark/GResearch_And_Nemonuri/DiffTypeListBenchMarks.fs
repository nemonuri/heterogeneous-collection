namespace GResearch_And_Nemonrui

open System
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Attributes
open HCollections
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives

module D = Nemonuri.Collections.Heterogeneous.DiffTypeLists

[<MemoryDiagnoser; DisassemblyDiagnoser>]
[<ShortRunJob(RuntimeMoniker.Net10_0)>]
[<ShortRunJob(RuntimeMoniker.Net80)>]
[<ShortRunJob(RuntimeMoniker.Net472)>]
type DiffTypeListBenchMarks () =

    let typeListFolder = 
        { new IFolder<Type list> with
            member _.Step (acc: Type list, elem: 'T): Type list = typeof<'T>::acc }
    
    let mkList1() =
        D.empty
        |> D.cons<int,_,_>
        |> D.cons<string,_,_>
        |> D.cons<bool,_,_>
        |> D.cons<char,_,_>
        |> D.cons<uint32,_,_>

    let expected = [typeof<int>; typeof<string>; typeof<bool>; typeof<char>; typeof<uint32>] @ [typeof<float>; typeof<unit>; typeof<nativeint>]

    let guardExpected (expected: Type list) (actual: Type list) =
        List.forall2 (fun t1 t2 -> t1 = t2) expected actual
        |> function
            | true -> actual
            | false -> failwith "not expected"

    static member mkList2<'a when 'a :> D.IPredecessor<'a> and 'a : unmanaged>() =
        D.create<'a>
        |> D.cons<float,_,_>
        |> D.cons<unit,_,_>
        |> D.cons<nativeint,_,_>
    
    [<Benchmark>]
    member this.Nemonuri() =
        mkList1()
        |> D.append (DiffTypeListBenchMarks.mkList2<_>())
        |> D.fold typeListFolder []
        |> guardExpected expected
