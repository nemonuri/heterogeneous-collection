module Nemonuri.Collections.Heterogeneous.UnitTests.MinimalHLists

open Xunit
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.MinimalHLists

[<Fact>]
let ``(length empty) is 0``() =
    let actual = (length empty)
    Assert.Equal(0, actual)

[<Fact>]
let ``empty is empty``() =
    let actual = empty |> isEmpty
    Assert.True(actual)

let private consAndDecons (x: 'a) =
    empty |> cons x |> decons |> fst

[<Fact>]
let ``consAndDecons is identity - Test1``() =
    let x = 1
    Assert.Equal(x, consAndDecons x)

[<Fact>]
let ``consAndDecons is identity - Test2``() =
    let x = "Hello!"
    Assert.Equal(x, consAndDecons x)

[<Fact>]
let ``consAndDecons is identity - Test3``() =
    let x = {| Name ="John"; Age = 23; Rate = 9.34 |}
    Assert.Equal(x, consAndDecons x)

let private consAndDecons2 (x1: 'a1, x2: 'a2) =
    let l = empty |> cons x1 |> cons x2 in
    match decons l with
    | r2, l ->
    match decons l with
    | r1, _ -> r1, r2

[<Fact>]
let ``consAndDecons2 is identity - Test1``() =
    let xs = 1, 2 in
    Assert.Equal(xs, consAndDecons2 xs)

[<Fact>]
let ``consAndDecons2 is identity - Test2``() =
    let xs = 1, "Two" in
    Assert.Equal(xs, consAndDecons2 xs)

[<Fact>]
let ``consAndDecons2 is identity - Test3``() =
    let xs = "One", 2 in
    Assert.Equal(xs, consAndDecons2 xs)

[<Fact>]
let ``consAndDecons2 is identity - Test4``() =
    let xs = "One", "Two" in
    Assert.Equal(xs, consAndDecons2 xs)

let private consAndDecons3 (x1: 'a1, x2: 'a2, x3: 'a3) =
    let l = empty |> cons x1 |> cons x2 |> cons x3 in
    match decons l with
    | r3, l ->
    match decons l with
    | r2, l -> 
    match decons l with
    | r1, _ -> r1, r2, r3

[<Fact>]
let ``consAndDecons3 is identity``() =
    let xs = 5.6, struct ("Hello", Some "World"), Ok '6' in
    Assert.Equal(xs, consAndDecons3 xs)

let private stringFolder = 
    { new IFolder<string> with 
        member _.Fold (str: string) (elem: 'T): string = 
            if str.Length = 0 then 
                str + (elem |> box |> _.ToString())
            else
                str + " " + (elem |> box |> _.ToString()) }

[<Fact>]
let ``fold and stringFolder works expected``() =
    let l = empty |> cons 1 |> cons "Haha" |> cons true |> cons (System.Object())
    let expected = "System.Object True Haha 1"
    let actual = Folders.fold stringFolder "" l
    Assert.Equal(expected, actual)
