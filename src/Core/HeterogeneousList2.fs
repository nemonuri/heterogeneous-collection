namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type HeterogeneousList2<'TPred when 'TPred :> IFolderVisitable<'TPred>> = private { Pred: 'TPred }


module HeterogeneousLists2 = begin

    [<NoEquality; NoComparison>]
    type Empty = struct 

        static member Accept (folder: IFolder<'TState>, acc: 'TState, elem: Empty) = acc

        interface IFolderVisitable<Empty> with
            member _.Accept (folder, acc, elem) = Empty.Accept(folder, acc, elem)

    end

    let empty: HeterogeneousList2<Empty> = { Pred = Empty() }

    let isEmpty (l: HeterogeneousList2<'a>) = typeof<'a> = typeof<Empty>


    let private fold_core folder acc (l: HeterogeneousList2<_>) = l.Pred.Accept(folder, acc, l.Pred)

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type Pair<'hd, 'pred when 'pred :> IFolderVisitable<'pred>> = private { Head: 'hd; Tail: HeterogeneousList2<'pred> } with

        static member Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'pred>) = 
            let newAcc = folder.Step<'hd>(acc, elem.Head)
            elem.Tail |> fold_core folder newAcc

        interface IFolderVisitable<Pair<'hd, 'pred>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'pred>.Accept(folder, acc, elem)

    end

    let private tryPredV (l: HeterogeneousList2<_>) =
        if isEmpty l then
            ValueNone
        else
            l.Pred |> ValueSome

    let private toPair (l: HeterogeneousList2<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> _.Head

    let tail l = toPair l |> _.Tail
    

    let cons (hd: 'hd) (l: HeterogeneousList2<'tl>) =
        let pred = { Pair.Head = hd; Pair.Tail = l; } in
        { HeterogeneousList2.Pred = pred }

    let fold folder seed l = fold_core folder seed l

    let private folderForLength = { new IFolder<int> with member _.Step (acc: int, _: 'T): int = acc + 1 }

    let length l = fold folderForLength 0 l


end