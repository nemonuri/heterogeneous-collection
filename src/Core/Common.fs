namespace Nemonuri.Collections.Heterogeneous

open System
open System.Runtime.CompilerServices

type IFolder<'TState> = interface

    abstract member Fold<'T> : 'TState -> 'T -> 'TState

end

type IFolderVisitable<'TContext> = interface

    abstract member Accept<'TState, 'T> : folder:IFolder<'TState> * acc:'TState * elem:'T -> 'TState

end

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal UntypedItem =
| Primitive of p: int64
| Object of o: obj

module internal UntypedItems = begin

    let ofTyped (x: 'a) : UntypedItem =
        if typeof<'a>.IsPrimitive && sizeof<'a> <= sizeof<int64> then
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

end


[<RequireQualifiedAccess>]
[<Struct>]
type TypeHandle<'TType> = internal { InnerValue: RuntimeTypeHandle } with

    member x.Value = x.InnerValue

end

module TypeHandles = begin

    let ofType<'T> : TypeHandle<'T> = { InnerValue = typeof<'T>.TypeHandle }

    let tryToTypedV<'T> (rth: RuntimeTypeHandle) : voption<TypeHandle<'T>> = 
        if typeof<'T>.TypeHandle = rth then
            { TypeHandle.InnerValue = rth } |> ValueSome
        else
            ValueNone

end