namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives
module H = NonEmptyHeterogeneousLists

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type NonEmptyDiffList<'TPred, 'TAnc> = 
    private { 
        Arrow: NonEmptyHeterogeneousList<'TAnc> -> NonEmptyHeterogeneousList<'TPred>;
    }


module NonEmptyDiffLists = begin

    let private ofArrow arrow = { NonEmptyDiffList.Arrow = arrow }

    type IPredecessor = H.IPredecessor

    type Singleton<'a> = H.Singleton<'a>

    type Pair<'hd,'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl : struct> = H.Pair<'hd,'tl>
    
    let assume<'anc 
                when 'anc :> IPredecessorPremise<'anc> and 'anc :> IPredecessor and 'anc : struct> 
            : NonEmptyDiffList<'anc,'anc>
        = id |> ofArrow

#if false
    let singleton x  : NonEmptyDiffList<Singleton<'a>,Singleton<'a>> =
        let konst x0 y = H.singleton x0 in
        konst x |> ofArrow
#endif

    let assumeSingleton<'a> = assume<Singleton<'a>>

    let cons (hd: 'hd) (tl: NonEmptyDiffList<'tl, 'anc>) : NonEmptyDiffList<Pair<'hd,'tl>,'anc> = 
        tl.Arrow >> H.cons hd |> ofArrow
    
    let append<'pred, 'anc1, 'anc2
                when 'pred :> IPredecessor
                and 'anc1 :> IPredecessor
                and 'anc2 :> IPredecessor>
        (first: NonEmptyDiffList<'pred, 'anc1>) (second: NonEmptyDiffList<'anc1, 'anc2>) =
        second.Arrow >> first.Arrow |> ofArrow

#if false
    let appendSingleton l x = 
        singleton x |> append l
#endif

    let toNonEmptyHeterogeneousList (l: NonEmptyDiffList<'pred,Singleton<'a>>) x =
        H.singleton x |> l.Arrow

end