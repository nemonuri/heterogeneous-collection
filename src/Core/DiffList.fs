namespace Nemonuri.Collections.Heterogeneous

open System.Runtime.CompilerServices
open Nemonuri.Collections.Heterogeneous.Primitives
module Hl = HeterogeneousLists

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type DiffList<'TPred, 'TAnc> = 
    private { 
        Pred: 'TPred; 
        mutable Anc: IInRefProvider<'TAnc> | null;
    }
    with
    
        member private this.GetInRef() : inref<'TPred> =
            if typeof<'TPred> = typeof<'TAnc> then
                match this.Anc with
                | :? IInRefProvider<'TPred> as p -> &p.InRef
                | _ -> &(Unsafe.NullRef<_>())
            else
                &this.Pred


        interface IInRefProvider<'TPred> with

            member x.InRef = &x.GetInRef()

    end


module DiffLists = begin

    type IPredecessor = Hl.IPredecessor

    let assume<'anc when 'anc :> IPredecessor> : DiffList<'anc, 'anc> = { Pred = Unchecked.defaultof<_>; Anc = null }
        
    let private isAssume (l: DiffList<'pred, 'anc>) = typeof<'pred> = typeof<'anc>

    [<NoEquality; NoComparison; Sealed>]
    type private EmptyInRef = class

        static let s_empty = Hl.Empty()

        static member private EmptyInRef = &s_empty

        private new() = {}

        static member Instance = EmptyInRef()

        interface IInRefProvider<Hl.Empty> with

            member x.InRef = &EmptyInRef.EmptyInRef

    end

    let empty : DiffList<Hl.Empty, Hl.Empty> = { Pred = Hl.Empty(); Anc = EmptyInRef.Instance }

    let length (l: DiffList<'pred, 'anc>) = Predecessors.length<'pred>

    let isEmpty l = (length l) = 0


    let head (l: DiffList<Hl.Pair<_,_>,_>) = l.Pred.Head

    let tail (l: DiffList<Hl.Pair<_,_>,_>) = l.Pred.Tail :?> DiffList<_,_>

    let cons (hd: 'hd) (tl: DiffList<'tl, 'anc>) =
        isAssume tl

    let fold folder (seed: 'state) (l: DiffList<'pred, 'anc>) =
        Hl.fold folder seed l.InnerList


    let append<'pred, 'anc1, 'anc2
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor>
        (first: DiffList<'pred, 'anc1>) (second: DiffList<'anc1, 'anc2>) : DiffList<'pred, 'anc2> =
        match first.InnerList with
        | null -> second |> unbox
        | il ->
            
            


end