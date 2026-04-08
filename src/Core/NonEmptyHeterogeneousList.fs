namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type NonEmptyHeterogeneousList<'TPred> = private { Pred: 'TPred }


module NonEmptyHeterogeneousLists = begin

    type IPredecessor = interface end

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type Singleton<'T> = private { Value: 'T } with

        static member private Length = 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Singleton<'T>) = 
            folder.Step<'T>(acc, elem.Value)
        
        interface IPredecessor

        interface IPredecessorPremise<Singleton<'T>> with

            member _.Length = Singleton<'T>.Length

            member _.Accept (folder, acc, elem) = Singleton<'T>.Accept(folder, acc, elem)

    end

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl : struct> = 
        private { Head: 'hd; Tail: NonEmptyHeterogeneousList<'tl> } with

        static member private Length = Predecessors.tailLength<'tl, Pair<'hd, 'tl>> + 1

        static member private Accept (folder: IFolder<'state>, acc: 'state, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, elem.Head)
            Predecessors.visitTail<'tl,Pair<'hd, 'tl>,'state> folder newAcc elem.Tail.Pred


        interface IPredecessor

        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'tl>.Accept(folder, acc, elem)
        
            member _.Length = Pair<'hd, 'tl>.Length

    end

    let singleton (x: 'a) = 
        let sgt = { Singleton.Value = x } in
        { NonEmptyHeterogeneousList.Pred = sgt }

    let length (l: NonEmptyHeterogeneousList<'pred>) = Predecessors.length<'pred>

    let isSingleton l = (length l) = 1

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type private PredResult<'TPred> =
    | Singleton of 'TPred
    | Pair of 'TPred

    let private toPred (l: NonEmptyHeterogeneousList<_>) =
        if isSingleton l then
            l.Pred |> PredResult.Singleton
        else
            l.Pred |> PredResult.Pair

    let private toPair (l: NonEmptyHeterogeneousList<Pair<'hd, 'tl>>) =
        match toPred l with
        | PredResult.Singleton _ -> failwith "Unreachable"
        | PredResult.Pair pair -> pair
    
    let head l = toPair l |> _.Head
    let tail l = toPair l |> _.Tail


    let inline private cons_aux<'tl when 'tl :> IPredecessor> (l: NonEmptyHeterogeneousList<'tl>) = l

    let cons (hd: 'hd) (l: NonEmptyHeterogeneousList<'tl>) =
        let pred = { Pair.Head = hd; Pair.Tail = l |> cons_aux; } in
        { NonEmptyHeterogeneousList.Pred = pred }

    let fold folder (seed: 'state) (l: NonEmptyHeterogeneousList<'pred>) = Predecessors.accept folder seed l.Pred

end


