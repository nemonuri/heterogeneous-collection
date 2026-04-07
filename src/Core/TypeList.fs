namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Collections.Heterogeneous.Primitives

module D = Nemonuri.Collections.Heterogeneous.DiffTypeLists

module TypeLists = begin   

    type Empty = D.Empty

    type TypeList<'pred> = DiffTypeList<'pred, Empty>

    type Pair<'hd, 'tl
        when 'tl :> IPredecessorPremise<'tl> and 'tl :> D.IPredecessor and 'tl : struct> = D.Pair<'hd, 'tl>


    let empty : TypeList<Empty> = D.empty

    let length (l: TypeList<_>) = l |> D.length

    let isEmpty (l: TypeList<_>) = l |> D.isEmpty

    let head (l: TypeList<Pair<_,_>>) = l |> D.head

    let tail (l: TypeList<Pair<_,_>>) = l |> D.tail

    let cons<'hd, 'tl
                when 'tl :> IPredecessorPremise<'tl> and 'tl :> D.IPredecessor and 'tl : struct> 
                (tl: TypeList<'tl>) : TypeList<Pair<'hd, 'tl>> = 
        tl |> D.cons<'hd, 'tl, Empty>

    let fold folder acc (l: TypeList<_>) = l |> D.fold folder acc

end
