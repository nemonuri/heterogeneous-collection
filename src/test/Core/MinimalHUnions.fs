module Nemonuri.Collections.Heterogeneous.UnitTests.QuickUnions

open Xunit
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.MinimalHUnions.Patterns

module T = MinimalTypeLists
module U = MinimalHUnions

[<Fact>]
let Test1() =
    let u = T.empty |> T.cons<int,_> |> T.cons<bool,_> |> T.cons<string,_> |> U.make "Hi!"
    match u with
    | Value v -> Assert.Equal("Hi!", v)
    | Union u -> 
    match u with
    | Value v -> Assert.Fail()
    | Union u ->
    match u with
    | Value v -> Assert.Fail()
    | Union u -> Assert.Fail()

[<Fact>]
let Test2() =
    let u = T.empty |> T.cons<int,_> |> T.cons<bool,_> |> U.make true |> U.extend<string,_>
    match u with
    | Value v -> Assert.Fail()
    | Union u -> 
    match u with
    | Value v -> Assert.Equal(true, v)
    | Union u ->
    match u with
    | Value v -> Assert.Fail()
    | Union u -> Assert.Fail()

[<Fact>]
let Test3() =
    let u = T.empty |> T.cons<int,_> |> U.make 123 |> U.extend<bool,_>  |> U.extend<string,_>
    match u with
    | Value v -> Assert.Fail()
    | Union u -> 
    match u with
    | Value v -> Assert.Fail()
    | Union u ->
    match u with
    | Value v -> Assert.Equal(123, v)
    | Union u -> Assert.Fail()

