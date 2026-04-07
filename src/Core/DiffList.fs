namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

module private DiffListInternals = begin

    type IInnerRefProvider<'a> = interface

        abstract member InnerRef: byref<'a>

    end

end

open DiffListInternals

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type DiffList<'TPred, 'TAnc> = { mutable Pred: 'TPred } with

    interface IInnerRefProvider<'TPred> with

        member x.InnerRef = &x.Pred


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
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : struct> = 
        private { Head: 'hd; Tail: IInnerRefProvider<'tl> } with

        static member private Length = Predecessors.tailLength<'tl, Pair<'hd, 'tl>> + 1

        static member private Accept (folder: IFolder<'state>, acc: 'state, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, elem.Head)
            Predecessors.visitTail<'tl,Pair<'hd, 'tl>,'state> folder newAcc elem.Tail.InnerRef

        interface IPredecessor

        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'tl>.Accept(folder, acc, elem)
        
            member x.Length = Pair<'hd, 'tl>.Length

        interface IInnerRefProvider<'tl> with

            member x.InnerRef = &(x.Tail.InnerRef)

    end

    let assume<'anc when 'anc :> IPredecessor> : DiffList<'anc, 'anc> = { DiffList.Pred = Unchecked.defaultof<_> }

    let empty = assume<Empty>

    let length (l: DiffList<'pred, 'anc>) = Predecessors.length<'pred>

    let isEmpty l = (length l) = 0


    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = pair.Head

        let tail (pair: Pair<'hd, 'pred>) = pair.Tail

    end


    let private tryPredV (l: DiffList<'pred, 'anc>) =
        match isEmpty l with
        | true -> ValueNone
        | false -> ValueSome l.Pred
    
    let private toPair (l: DiffList<Pair<'hd, 'tl>, 'anc>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let tail (l: DiffList<Pair<'hd,'tl>,'anc>) = toPair l |> Pairs.tail :?> DiffList<'tl,'anc>

    let cons (hd: 'hd) (tl: DiffList<'tl, 'anc>) =
        let pair = { Pair.Head = hd; Pair.Tail = tl }
        { DiffList.Pred = pair }

    let fold folder (seed: 'state) (l: DiffList<'pred, 'anc>) =
        Predecessors.accept folder seed l.Pred

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison>]
    type private Appender<'pred, 'anc1, 'anc2 when 'pred :> IPredecessor and 'anc2 :> IPredecessor> 
        = { Value: 'anc1 } with

        interface IFolder<DiffList<'pred, 'anc2>> with
            member x.Step (acc: DiffList<'pred,'anc2>, elem: 'T): DiffList<'pred,'anc2> = 
                match box elem with
                | :? IInnerRefProvider<'anc1> as p ->
                    p.InnerRef <- x.Value;
                | _ -> ()
                acc

    end


    let append<'pred, 'anc1, 'anc2
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor>
        (first: DiffList<'pred, 'anc1>) (second: DiffList<'anc1, 'anc2>) : DiffList<'pred, 'anc2> =
        if typeof<'pred> = typeof<'anc1> then
            second |> unbox
        else
            


end