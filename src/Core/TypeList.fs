namespace Nemonuri.Collections.Heterogeneous


module D = Nemonuri.Collections.Heterogeneous.DiffTypeLists


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type TypeList<'TPred> = private { Diff: DiffTypeList<'TPred, D.Empty> }


module TypeLists = begin   

    type Empty = D.Empty

    type Pair<'hd, 'pred
        when 'pred :> D.IPredecessor<'pred>
        and 'pred : unmanaged> = D.Pair<'hd, 'pred>


    let ofDiff d = { TypeList.Diff = d }

    let toDiff (l: TypeList<_>) = l.Diff

    let empty = D.empty |> ofDiff

    let isEmpty l = l |> toDiff |> D.isEmpty

    let head l = l |> toDiff |> D.head

    let tail l = l |> toDiff |> D.tail |> ofDiff

    let cons<'hd, 'pred
                when 'pred :> D.IPredecessor<'pred>
                and 'pred : unmanaged> tl = 
        tl |> toDiff |> D.cons<'hd, 'pred, Empty> |> ofDiff

    let fold folder acc l = l |> toDiff |> D.fold folder acc

    let length l = l |> toDiff |> D.length


end
