namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type NonEmptyDiffTypeList<'TPred, 'TAnc> = private | T

module NonEmptyDiffTypeLists = begin

    open Unchecked

    type IPredecessor = interface end

    [<NoEquality; NoComparison>]
    type Singleton<'T> = struct

        static member private Length = 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Singleton<'T>) = 
            let newAcc = folder.Step<'T>(acc, defaultof<_>) in
            newAcc
        
        interface IPredecessor

        interface IPredecessorPremise<Singleton<'T>> with
            member _.Length = Singleton<'T>.Length

            member _.Accept (folder, acc, elem) = Singleton<'T>.Accept(folder, acc, elem)

    end

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison>]
    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : struct> = struct

        static member private Length = Predecessors.tailLength<'tl, Pair<'hd, 'tl>> + 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'tl>) = 
            let newAcc = folder.Step<'hd>(acc, defaultof<_>) in
            Predecessors.visitTail<'tl, Pair<'hd, 'tl>, _> folder newAcc defaultof<'tl>

        interface IPredecessor

        interface IConstructedPredecessorPremise<'tl, Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'tl>.Accept(folder, acc, elem)
        
            member x.Length = Pair<'hd, 'tl>.Length

    end


    let assume<'anc when 'anc :> IPredecessor> : NonEmptyDiffTypeList<'anc,'anc> = NonEmptyDiffTypeList.T   
    
    let singleton<'a> = assume<Singleton<'a>>

    let length (l: NonEmptyDiffTypeList<'pred,'anc>) = Predecessors.length<'pred>

    let isSingleton l = (length l) = 1

    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<'hd, 'pred>) : NonEmptyDiffTypeList<'pred, 'anc> = NonEmptyDiffTypeList.T

    end

    let private tryPredV (l: NonEmptyDiffTypeList<'pred, 'anc>) =
        match isSingleton l with
        | true -> ValueNone
        | false -> ValueSome (new 'pred())
    
    let private toPair (l: NonEmptyDiffTypeList<Pair<'hd, 'pred>, 'anc>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let private tail_core l = toPair l |> Pairs.tail

    let tail (l: NonEmptyDiffTypeList<Pair<'hd, 'pred>,'anc>) : NonEmptyDiffTypeList<'pred, 'anc> = tail_core l

    let cons<'hd, 'tl, 'anc
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> IPredecessor and 'tl : struct> 
        (tl: NonEmptyDiffTypeList<'tl,'anc>) : NonEmptyDiffTypeList<Pair<'hd, 'tl>, 'anc> = 
        NonEmptyDiffTypeList.T

    let append<'pred, 'anc1, 'anc2
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor>
        (first: NonEmptyDiffTypeList<'pred, 'anc1>) (second: NonEmptyDiffTypeList<'anc1, 'anc2>) : NonEmptyDiffTypeList<'pred, 'anc2> = NonEmptyDiffTypeList.T

    
    let fold folder (seed: 'state) (l: NonEmptyDiffTypeList<'pred, 'anc>) = Predecessors.accept folder seed defaultof<'pred>


end