module Nemonuri.Collections.Heterogeneous.UnitTests.QuickHLists

open Xunit
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.QuickHLists

type Ty<'T> = struct end

let ty<'a> = Ty<'a>()

let (|Type|_|) (_: Ty<'a>) (x: 'b) =
    if typeof<'a> = typeof<'b> then
        ValueSome (RetypeTheory.UnsafeRetype<'b,'a>(&x))
    else
        ValueNone


let isLesserThan (n: int) (x: 'a) : bool =
    let _int = ty<int> in
    let _float = ty<float> in
    let _string = ty<string> in
    match x with
    | Type _int v -> n < v
    | Type _float v -> n < int v
    | Type _string v -> 
        let ok, r = System.Int32.TryParse(v) in
        if ok then n < r else false
    | _ -> false

type IPredicate = interface

    abstract member Invoke<'T>: 'T -> bool

end

let toIntFolder (p: IPredicate) = 
    { new IFolder<int> with 
        member _.Step (acc: int, elem: 'T): int = 
            match p.Invoke(elem) with
            | true -> acc + 1
            | false -> acc }

[<Fact>]
let Test1() =
    let l = empty |> cons 3 |> cons -2.5 |> cons "0" |> cons "1" |> cons 4u in
    let folder = toIntFolder { new IPredicate with member _.Invoke (x: 'T): bool = isLesserThan 0 x } in
    let actual = fold folder 0 l in
    Assert.Equal(2, actual)

