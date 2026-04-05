module Nemonuri.Collections.Heterogeneous.UnitTests.TypeLists

open Xunit
open System
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.TypeLists

module internal Fixtures = begin

    let list1 =
        empty
        |> cons<int,_>
        |> cons<string,_>
        |> cons<bool,_>
        |> cons<char,_>
        |> cons<uint32,_>

    let expected1 = [typeof<int>; typeof<string>; typeof<bool>; typeof<char>; typeof<uint32>]

    let typeListFolder = 
        { new IFolder<Type list> with
            member _.Step (acc: Type list, elem: 'T): Type list = typeof<'T>::acc }

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
let ``fold list1 by typeListFolder is expected``() =
    let actual = fold Fixtures.typeListFolder [] Fixtures.list1
    Assert.Equal<Type>(Fixtures.expected1, actual)
