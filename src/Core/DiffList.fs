namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type DiffList<'TPred, 'TAnc> = private { Pred: 'TPred }

module DiffLists = begin

    type IPredecessor = interface end

    [<NoEquality; NoComparison>]
    type Empty = struct

        static member private Length = 0

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Empty) = acc

        interface IPredecessor

        interface IPredecessorPremise<Empty> with
            member _.Length = Empty.Length

            member _.Accept (folder, acc, elem) = Empty.Accept(folder, acc, elem)

    end

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : (new:unit -> 'tl)> 
                = private { Head: 'hd; Tail: 'tl } with

        static member private Length = PredecessorTheory.GetTailLength<'tl, Pair<'hd, 'tl>>() + 1

        static member private Accept (folder: IFolder<'state>, acc: 'state, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, elem.Head)
            elem.Tail |> fold_core folder newAcc

        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with

    end

    and private Premise<'a when 'a :> IPredecessorPremise<'a> and 'a :> IPredecessor> = struct



    end







end