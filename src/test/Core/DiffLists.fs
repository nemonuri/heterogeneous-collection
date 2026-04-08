module Nemonuri.Collections.Heterogeneous.UnitTests.DiffLists

open Xunit
open System
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.Primitives
open Nemonuri.Collections.Heterogeneous.DiffLists


module private Fixtures = begin

    let folder = 
        { new IFolder<list<string>> with 
            member _.Step (acc: string list, elem: 'T): string list = ((box elem).ToString() |> defaultIfNull "")::acc  }


end

[<Fact>]
let Test1() =
    let list1 = assume |> cons 1 |> cons "2" in
    let list2 = assume |> cons true |> cons 'a' in
    let actual =
        list2
        |> append list1
        |> appendEmpty
        |> fold Fixtures.folder []
    let expect = ["True";"a";"1";"2"]
    Assert.Equal<string>(expect, actual)

