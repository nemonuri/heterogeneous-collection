namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type DiffTypeList<'TPred, 'TAnc> = private | T

module DiffTypeLists = begin

    open Unchecked

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
    [<NoEquality; NoComparison>]
    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : struct> = struct

        static member private Length = Predecessors.tailLength<'tl, Pair<'hd, 'tl>> + 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, defaultof<_>) in
            Predecessors.visitTail<'tl,Pair<'hd, 'tl>,_> folder newAcc defaultof<'tl>
        
        interface IPredecessor
        
        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'tl>.Accept(folder, acc, elem)
            member x.Length = Pair<'hd, 'tl>.Length

    end

    let assume<'anc when 'anc :> IPredecessor> : DiffTypeList<'anc,'anc> = DiffTypeList.T   
    
    let empty : DiffTypeList<Empty, Empty> = assume<Empty>

    let length (l: DiffTypeList<'pred, 'anc>) = Predecessors.length<'pred>

    let isEmpty (l: DiffTypeList<'pred,'anc>) = (length l) = 0


    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<'hd, 'pred>) : DiffTypeList<'pred, 'anc> = DiffTypeList.T

    end


    let private tryPredV (l: DiffTypeList<'pred, 'anc>) =
        match isEmpty l with
        | true -> ValueNone
        | false -> ValueSome (new 'pred())
    
    let private toPair (l: DiffTypeList<Pair<'hd, 'tl>, 'anc>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred


    let head l = toPair l |> Pairs.head

    let private tail_core l = toPair l |> Pairs.tail

    let tail (l: DiffTypeList<Pair<'hd, 'tl>,'anc>) : DiffTypeList<'tl, 'anc> = tail_core l

    let cons<'hd, 'tl, 'anc
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : struct> (tl: DiffTypeList<'tl, 'anc>) : DiffTypeList<Pair<'hd, 'tl>, 'anc> = DiffTypeList.T

    let append<'pred, 'anc1, 'anc2 
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor> 
        (first: DiffTypeList<'pred, 'anc1>) (second: DiffTypeList<'anc1, 'anc2>) : DiffTypeList<'pred, 'anc2> = DiffTypeList.T

    
    let fold folder (seed: 'state) (l: DiffTypeList<'pred,'anc>) = 
        Predecessors.accept folder seed defaultof<'pred>
        
    

end