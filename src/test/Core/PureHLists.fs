module Nemonuri.Collections.Heterogeneous.UnitTests.PureHLists

open Xunit
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.PureHLists


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


(* TODO: 좀 더 제대로 된 이름 짓기 *)
[<Fact>]
let Test1() =
    let folder = QuickHLists.toIntFolder { new QuickHLists.IPredicate with member _.Invoke (x: 'T): bool = QuickHLists.isLesserThan 0 x } in
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

