namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type HeterogeneousList<'TPred> = private { Pred: 'TPred }


module HeterogeneousLists = begin

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
                when 'tl :> IPredecessorPremise<'tl> and 'tl : struct> = 
        private { Head: 'hd; Tail: HeterogeneousList<'tl> } with

        static member private Length = Predecessors.tailLength<'tl, Pair<'hd, 'tl>> + 1

        static member private Accept (folder: IFolder<'state>, acc: 'state, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, elem.Head)
            Predecessors.visitTail<'tl,Pair<'hd, 'tl>,'state> folder newAcc elem.Tail.Pred

        interface IPredecessor

        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'tl>.Accept(folder, acc, elem)
        
            member x.Length = Pair<'hd, 'tl>.Length

    end


    let empty: HeterogeneousList<Empty> = { Pred = Empty() }
    let length (l: HeterogeneousList<'pred>) = Predecessors.length<'pred>

    let isEmpty (l: HeterogeneousList<'a>) = (length l) = 0


    let private tryPredV (l: HeterogeneousList<_>) =
        if isEmpty l then
            ValueNone
        else
            l.Pred |> ValueSome

    let private toPair (l: HeterogeneousList<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> _.Head

    let tail l = toPair l |> _.Tail
    
    let inline private cons_aux<'tl when 'tl :> IPredecessor> (l: HeterogeneousList<'tl>) = l

    let cons (hd: 'hd) (l: HeterogeneousList<'tl>) =
        let pred = { Pair.Head = hd; Pair.Tail = l |> cons_aux; } in
        { HeterogeneousList.Pred = pred }

    let fold folder (seed: 'state) (l: HeterogeneousList<'pred>) = Predecessors.accept folder seed l.Pred


end