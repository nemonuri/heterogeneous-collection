namespace Nemonuri.Collections.Heterogeneous

module D = Nemonuri.Collections.Heterogeneous.NonEmptyDiffTypeLists

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type NonEmptyTypeList<'TPred, 'TLast> = private { Diff: NonEmptyDiffTypeList<'TPred, D.Singleton<'TLast>> }

module NonEmptyTypeLists = begin

    let ofDiff d = { NonEmptyTypeList.Diff = d }

    let toDiff (l: NonEmptyTypeList<_,_>) = l.Diff

    let singleton<'a> = D.singleton<'a> |> ofDiff

    let isSingleton l = l |> toDiff |> D.isSingleton

    let head l = l |> toDiff |> D.head

    let tail l = l |> toDiff |> D.tail

    let cons<'hd, 'pred, 'last
                when 'pred :> D.IPredecessor<'pred>
                and 'pred : unmanaged> l =
        l |> toDiff |> D.cons<'hd, 'pred, D.Singleton<'last>> |> ofDiff

    let fold folder acc l = l |> toDiff |> D.fold folder acc

    let length l = l |> toDiff |> D.length

end


