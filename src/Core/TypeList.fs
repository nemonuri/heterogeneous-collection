namespace Nemonuri.Collections.Heterogeneous


module D = Nemonuri.Collections.Heterogeneous.DiffTypeLists

module TypeLists = begin   

    type Empty = D.Empty

    type TypeList<'pred> = DiffTypeList<'pred, Empty>

    type Pair<'hd, 'pred
        when 'pred :> D.IPredecessor<'pred>
        and 'pred : unmanaged> = D.Pair<'hd, 'pred>


    let empty : TypeList<Empty> = D.empty

    let isEmpty (l: TypeList<_>) = l |> D.isEmpty

    let head (l: TypeList<Pair<_,_>>) = l |> D.head

    let tail (l: TypeList<Pair<_,_>>) = l |> D.tail

    let cons<'hd, 'pred
                when 'pred :> D.IPredecessor<'pred>
                and 'pred : unmanaged> (tl: TypeList<'pred>) : TypeList<Pair<'hd, 'pred>> = 
        tl |> D.cons<'hd, 'pred, Empty>

    let fold folder acc (l: TypeList<_>) = l |> D.fold folder acc

    let length (l: TypeList<_>) = l |> D.length


end
