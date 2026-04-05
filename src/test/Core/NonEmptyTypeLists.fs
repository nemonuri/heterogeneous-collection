module Nemonuri.Collections.Heterogeneous.UnitTests.NonEmptyTypeLists


open Xunit
open System
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.NonEmptyTypeLists

module F = Nemonuri.Collections.Heterogeneous.UnitTests.TypeLists.Fixtures

module private Fixtures = begin

    let list1 =
        singleton<int>
        |> cons<string,_,_>
        |> cons<bool,_,_>
        |> cons<char,_,_>
        |> cons<uint32,_,_>

end

[<Fact>]
let Test1() =
    let actual =
        Fixtures.list1
        |> toTypeList
        |> TypeLists.fold F.typeListFolder []
    Assert.Equal<Type>(F.expected1, actual)
