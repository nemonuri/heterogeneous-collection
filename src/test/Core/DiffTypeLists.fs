module Nemonuri.Collections.Heterogeneous.UnitTests.DiffTypeLists


open Xunit
open System
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.DiffTypeLists

module private Fixtures = begin

    let typeListFolder = 
        { new IFolder<Type list> with
            member _.Step (acc: Type list, elem: 'T): Type list = typeof<'T>::acc }

    let list1 =
        empty
        |> cons<int,_,_>
        |> cons<string,_,_>
        |> cons<bool,_,_>
        |> cons<char,_,_>
        |> cons<uint32,_,_>

    let expected1 = [typeof<int>; typeof<string>; typeof<bool>; typeof<char>; typeof<uint32>]

    let list2<'a when 'a :> IPredecessor<'a> and 'a : unmanaged> =
        assume<'a>
        |> cons<float,_,_>
        |> cons<unit,_,_>
        |> cons<nativeint,_,_>

    let expected2 = [typeof<float>; typeof<unit>; typeof<nativeint>]

    let list2_at_list1 = list1 |> append list2

    let expected1_at_expected2 = expected1 @ expected2

end


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
let ``(head Fixtures.list1) is (typeof<uint32>)``() =
    let actual = (head Fixtures.list1)
    Assert.Equal(typeof<uint32>, actual)


[<Fact>]
let ``fold list1 by typeListFolder is expected1``() =
    let actual = fold Fixtures.typeListFolder [] Fixtures.list1
    Assert.Equal<Type>(Fixtures.expected1, actual)

[<Fact>]
let ``(append list2 empty |> length) is 3``() =
    let actual = append Fixtures.list2 empty |> length
    Assert.Equal(3, actual)

[<Fact>]
let ``fold list2_at_list1 by typeListFolder is expected2_at_expected1``() =
    let actual = fold Fixtures.typeListFolder [] Fixtures.list2_at_list1
    Assert.Equal<Type>(Fixtures.expected1_at_expected2, actual)
