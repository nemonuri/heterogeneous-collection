namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type NonEmptyDiffTypeList<'TPred, 'TAnc> = private | T

module NonEmptyDiffTypeLists = begin

    open Unchecked

    type IPredecessor<'pred> = interface

        inherit IFolderVisitable<'pred>

        abstract member Length: int

    end

    [<NoEquality; NoComparison>]
    type Singleton<'T> = struct

        static member private Length = 1

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Singleton<'T>) = 
            let newAcc = folder.Step<'T>(acc, defaultof<_>) in
            newAcc

        interface IPredecessor<Singleton<'T>> with
            member _.Length = Singleton<'T>.Length

            member _.Accept (folder, acc, elem) = Singleton<'T>.Accept(folder, acc, elem)

    end

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison>]
    type Pair<'hd, 'pred
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

    type private Pred<'pred 
                        when 'pred :> IPredecessor<'pred>
                        and 'pred : unmanaged> = 'pred

    let assume<'anc
                when 'anc :> IPredecessor<'anc>
                and 'anc : unmanaged> : NonEmptyDiffTypeList<'anc,'anc> = NonEmptyDiffTypeList.T   
    
    let singleton<'a> = assume<Singleton<'a>>

    let private toPred (l: NonEmptyDiffTypeList<Pred<'pred>, Pred<'anc>>) = defaultof<'pred>

    let isSingleton (l: NonEmptyDiffTypeList<'pred,'anc>) = (toPred l |> _.Length) = 1

    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<'hd, 'pred>) : NonEmptyDiffTypeList<'pred, 'anc> = NonEmptyDiffTypeList.T

    end

    let private tryPredV l =
        match isSingleton l with
        | true -> ValueNone
        | false -> ValueSome (toPred l)
    
    let private toPair (l: NonEmptyDiffTypeList<Pair<'hd, 'pred>, 'anc>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let private tail_core l = toPair l |> Pairs.tail

    let tail (l: NonEmptyDiffTypeList<Pair<'hd, 'pred>,'anc>) : NonEmptyDiffTypeList<'pred, 'anc> = tail_core l

    let cons<'hd, 'pred, 'anc
                when 'pred :> IPredecessor<'pred>
                and 'pred : unmanaged
                and 'anc :> IPredecessor<'anc>
                and 'anc : unmanaged> (tl: NonEmptyDiffTypeList<'pred, 'anc>) : NonEmptyDiffTypeList<Pair<'hd, 'pred>, 'anc> = NonEmptyDiffTypeList.T

    let append (first: NonEmptyDiffTypeList<'pred, Pred<'anc1>>) (second: NonEmptyDiffTypeList<'anc1, Pred<'anc2>>) : NonEmptyDiffTypeList<'pred, 'anc2> = NonEmptyDiffTypeList.T

    let private fold_core folder acc (l: NonEmptyDiffTypeList<_,_>) =
        let pred = toPred l in
        pred.Accept(folder, acc, pred)
    
    let fold folder acc l = fold_core folder acc l

    let length l = (toPred l).Length


end