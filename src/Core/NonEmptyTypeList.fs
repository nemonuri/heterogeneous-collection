namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives
module D = Nemonuri.Collections.Heterogeneous.NonEmptyDiffTypeLists
module Tl = Nemonuri.Collections.Heterogeneous.TypeLists

module NonEmptyTypeLists = begin

    type Singleton<'T> = D.Singleton<'T>

    type NonEmptyTypeList<'TPred, 'TLast> = NonEmptyDiffTypeList<'TPred, Singleton<'TLast>>

    type Pair<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> D.IPredecessor and 'tl : (new:unit -> 'tl)> = D.Pair<'hd, 'tl>


    let singleton<'a> : NonEmptyTypeList<Singleton<'a>,'a> = D.singleton<'a>

    let isSingleton (l: NonEmptyTypeList<_,_>) = l |> D.isSingleton

    let head (l: NonEmptyTypeList<Pair<_,_>,_>) = l |> D.head

    let tail (l: NonEmptyTypeList<Pair<'hd, 'pred>, 'last>) : NonEmptyTypeList<'pred, 'last> = l |> D.tail

    let cons<'hd, 'tl, 'last
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> D.IPredecessor and 'tl : (new:unit -> 'tl)> 
                (l: NonEmptyTypeList<_,_>) : NonEmptyTypeList<Pair<_,_>,_> =
        l |> D.cons<'hd, 'tl, Singleton<'last>>

    let fold folder acc (l: NonEmptyTypeList<_,_>) = l |> D.fold folder acc

    let length (l: NonEmptyTypeList<_,_>) = l |> D.length


    type Premise' = struct

        static member inline Call prem pred l =
            let inline call (prem0: ^p1) (pred0: ^p2) (l0: ^l) = ((^p1 or ^p2) : (static member ToTypeList : _*_*_ -> _) prem0, pred0, l0) 
            call prem pred l

        static member ToTypeList(prem :_, pred: Singleton<'TLast>, l: NonEmptyTypeList<Singleton<'TLast>, 'TLast>) = 
            Tl.empty 
            |> Tl.cons<'TLast,_>

        static member inline ToTypeList(prem :_, pred: Pair<'THead, ^TPred>, l: NonEmptyTypeList<Pair<'THead, ^TPred>, 'TLast>) = 
            l
            |> tail
            |> Premise'.Call prem Unchecked.defaultof<^TPred>
            |> Tl.cons<'THead,_>

    end

    let inline toTypeList (l: NonEmptyTypeList<^TPred, 'TLast>) =
        l
        |> Premise'.Call Unchecked.defaultof<Premise'> Unchecked.defaultof<^TPred> 

end

