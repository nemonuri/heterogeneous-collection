#nowarn "42"

module Nemonuri.Collections.Heterogeneous.UnitTests.HeterogeneousLists

open Xunit
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.HeterogeneousLists


module private Fixtures = begin

    let list1 =
        empty 
        |> cons 3 
        |> cons -2.5 
        |> cons "0" 
        |> cons "1" 
        |> cons 4u

    let list2 =
        empty
        |> cons ()

end

type Ty<'T> = struct end

let ty<'a> = Ty<'a>()

let private retype<'T,'U> (x:'T) : 'U = (# "" x : 'U #)

let (|Type|_|) (_: Ty<'a>) (x: 'b) =
    if typeof<'a> = typeof<'b> then
        ValueSome (retype<'b,'a> x)
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

(* TODO: 좀 더 제대로 된 이름 짓기 *)
[<Fact>]
let Test1() =
    let folder = toIntFolder { new IPredicate with member _.Invoke (x: 'T): bool = isLesserThan 0 x } in
    let actual = fold folder 0 Fixtures.list1 in
    Assert.Equal(2, actual)

[<Fact>]
let ``empty is empty``() =
    let actual = empty |> isEmpty
    Assert.True(actual)

[<Fact>]
let ``(length empty) is 0``() =
    let actual = (length empty)
    Assert.Equal(0, actual)

[<Fact>]
let ``(length Fixtures.list1) is 5``() =
    let actual = (length Fixtures.list1)
    Assert.Equal(5, actual)

[<Fact>]
let ``(head Fixtures.list1) is (uint32 4)``() =
    let actual = (head Fixtures.list1)
    Assert.Equal((uint32 4), Assert.IsType<uint32>(actual))

[<Fact>]
let ``length of tail is (length of original - 1) - Test1``() =
    let original = Fixtures.list1
    let expected = (length original) - 1
    let actual = (tail original |> length)
    Assert.Equal(expected, actual)

[<Fact>]
let ``length of tail is (length of original - 1) - Test2``() =
    let original = Fixtures.list2
    let expected = (length original) - 1
    let actual = (tail original |> length)
    Assert.Equal(expected, actual)

