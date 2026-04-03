#nowarn "42"

namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type DiffTypeList<'TPred, 'TAnc> = private | T

module DiffTypeLists = begin

    open Unchecked

    type IPredecessor<'pred> = interface

        inherit IFolderVisitable<'pred>

        abstract member Length: int

    end
    
    and [<NoEquality; NoComparison>]
        Empty = struct

        static member private Length = 0

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Empty) = acc

        interface IPredecessor<Empty> with
            member _.Length = Empty.Length

            member _.Accept (folder, acc, elem) = Empty.Accept(folder, acc, elem)

    end

    and [<RequireQualifiedAccess>]
        [<NoEquality; NoComparison>]
        Pair<'hd, 'pred
                when 'pred :> IPredecessor<'pred>
                and 'pred : unmanaged> = struct

        static member internal Pred = defaultof<'pred>

        static member internal Length = Pair<'hd, 'pred>.Pred.Length + 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'pred>) = 
            let newAcc = folder.Step<'hd>(acc, defaultof<_>) in
            let pred = Pair<'hd, 'pred>.Pred in
            pred.Accept(folder, newAcc, pred)

        interface IFolderVisitable<Pair<'hd, 'pred>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'pred>.Accept(folder, acc, elem)
        
        interface IPredecessor<Pair<'hd, 'pred>> with
            member x.Length = Pair<'hd, 'pred>.Length

    end

    let assume<'anc
                when 'anc :> IPredecessor<'anc>
                and 'anc : unmanaged> : DiffTypeList<'anc,'anc> = DiffTypeList.T   
    
    let empty : DiffTypeList<Empty, Empty> = assume<Empty>

    let private toPred<'pred, 'anc 
                        when 'pred :> IPredecessor<'pred>
                        and 'pred : unmanaged> (l: DiffTypeList<'pred, 'anc>) = defaultof<'pred>

    let isEmpty (l: DiffTypeList<'pred,'anc>) = (toPred l |> _.Length) = 0


    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<'hd, 'pred>) : DiffTypeList<'pred, 'anc> = DiffTypeList.T

    end

    let private tryPredV l =
        match isEmpty l with
        | true -> ValueNone
        | false -> ValueSome (toPred l)
    
    let private toPair (l: DiffTypeList<Pair<'hd, 'pred>, 'anc>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred


    let head l = toPair l |> Pairs.head

    let private tail_core l = toPair l |> Pairs.tail

    let tail (l: DiffTypeList<Pair<'hd, 'pred>,'anc>) : DiffTypeList<'pred, 'anc> = tail_core l

    let cons<'hd, 'pred, 'anc
                when 'pred :> IPredecessor<'pred>
                and 'pred : unmanaged> (tl: DiffTypeList<'pred, 'anc>) : DiffTypeList<Pair<'hd, 'pred>, 'anc> = DiffTypeList.T

    let append (first: DiffTypeList<'pred, 'anc1>) (second: DiffTypeList<'anc1, 'anc2>) : DiffTypeList<'pred, 'anc2> = DiffTypeList.T

    let private fold_core folder acc (l: DiffTypeList<_,_>) =
        let pred = toPred l in
        pred.Accept(folder, acc, pred)
    
    let fold folder acc l = fold_core folder acc l

    let length l = (toPred l).Length

end