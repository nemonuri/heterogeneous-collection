namespace Nemonuri.Collections.Heterogeneous

module D = Nemonuri.Collections.Heterogeneous.NonEmptyDiffTypeLists
module Tl = Nemonuri.Collections.Heterogeneous.TypeLists

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type NonEmptyTypeList<'TPred, 'TLast> = private { Diff: NonEmptyDiffTypeList<'TPred, D.Singleton<'TLast>> }

module NonEmptyTypeLists = begin

    type Singleton<'T> = D.Singleton<'T>

    type Pair<'hd, 'pred
        when 'pred :> D.IPredecessor<'pred>
        and 'pred : unmanaged> = D.Pair<'hd, 'pred>

    let ofDiff d = { NonEmptyTypeList.Diff = d }

    let toDiff (l: NonEmptyTypeList<_,_>) = l.Diff

    let singleton<'a> = D.singleton<'a> |> ofDiff

    let isSingleton l = l |> toDiff |> D.isSingleton

    let head l = l |> toDiff |> D.head

    let tail l = l |> toDiff |> D.tail |> ofDiff

    let cons<'hd, 'pred, 'last
                when 'pred :> D.IPredecessor<'pred>
                and 'pred : unmanaged> l =
        l |> toDiff |> D.cons<'hd, 'pred, Singleton<'last>> |> ofDiff

    let fold folder acc l = l |> toDiff |> D.fold folder acc

    let length l = l |> toDiff |> D.length


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

