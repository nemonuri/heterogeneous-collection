namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives
module H = HeterogeneousLists

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type DiffList<'TPred, 'TAnc> = 
    private { 
        Arrow: HeterogeneousList<'TAnc> -> HeterogeneousList<'TPred>;
    }


module DiffLists = begin

    let private ofArrow arrow = { DiffList.Arrow = arrow }

    type IPredecessor = H.IPredecessor

    type Empty = H.Empty

    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl : struct> = H.Pair<'hd, 'tl>

    let assume<'anc 
                when 'anc :> IPredecessorPremise<'anc> and 'anc :> IPredecessor and 'anc : struct> : DiffList<'anc,'anc>
        = id |> ofArrow

    let empty = assume<Empty>

    let toHeterogeneousList (l: DiffList<'pred, Empty>) = l.Arrow H.empty

    let private konst (l: HeterogeneousList<_>) (e: HeterogeneousList<Empty>) = l

    let ofHeterogeneousList (l: HeterogeneousList<_>) = l |> konst |> ofArrow

    let length l = l |> toHeterogeneousList |> H.length

    let isEmpty l = l |> toHeterogeneousList |> H.isEmpty

    let cons (hd: 'hd) (tl: DiffList<'tl, 'anc>) : DiffList<Pair<'hd,'tl>,'anc> = 
        tl.Arrow >> H.cons hd |> ofArrow


    let head (l: DiffList<Pair<_,_>,Empty>) = l |> toHeterogeneousList |> H.head

    let tail (l: DiffList<Pair<_,_>,Empty>) = l |> toHeterogeneousList |> H.tail |> ofHeterogeneousList


    let fold folder seed l = 
        l |> toHeterogeneousList |> H.fold folder seed


    let append<'pred, 'anc1, 'anc2
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor>
        (first: DiffList<'pred, 'anc1>) (second: DiffList<'anc1, 'anc2>) : DiffList<'pred, 'anc2> =
        second.Arrow >> first.Arrow |> ofArrow

    let appendEmpty dl = append dl empty


end