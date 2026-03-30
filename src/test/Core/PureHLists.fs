module Nemonuri.Collections.Heterogeneous.UnitTests.PureHLists

open Xunit
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.PureHLists

(* TODO: 좀 더 제대로 된 테스트 만들기 *)

[<Fact>]
let Test1() =
    let l = empty |> cons 3 |> cons -2.5 |> cons "0" |> cons "1" |> cons 4u in
    let folder = QuickHLists.toIntFolder { new QuickHLists.IPredicate with member _.Invoke (x: 'T): bool = QuickHLists.isLesserThan 0 x } in
    let actual = fold folder 0 l in
    Assert.Equal(2, actual)