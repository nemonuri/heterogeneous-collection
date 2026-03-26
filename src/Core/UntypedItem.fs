namespace Nemonuri.Collections.Heterogeneous

open System.Runtime.CompilerServices

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal UntypedItem =
| Primitive of p: int64
| Object of o: obj

module internal UntypedItems =

    let ofTyped (x: 'a) : UntypedItem =
        if typeof<'a>.IsPrimitive then
            let mutable v : int64 = 0L
            Unsafe.WriteUnaligned(&Unsafe.As<_,_>(&v), x)
            v |> UntypedItem.Primitive
        else
            box x |> UntypedItem.Object
    
    let unsafeToTyped<'a> (item: UntypedItem) : 'a =
        match item with
        | UntypedItem.Primitive p -> 
            let mutable v = p
            Unsafe.ReadUnaligned<'a>(&Unsafe.As<_,_>(&v))
        | UntypedItem.Object o -> unbox o
