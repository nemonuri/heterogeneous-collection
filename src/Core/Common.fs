namespace Nemonuri.Collections.Heterogeneous

open System
open System.Runtime.CompilerServices
open Nemonuri.Collections.Heterogeneous.Primitives

module Folders = begin

    let trySpecializeVisitableV<'ctx> (x: IFolderVisitable) = 
        let ok, result = FolderTheory.TrySpecialize<'ctx>(x)
        if ok then ValueSome result else ValueNone
    
    let specializeVisitable<'ctx> x = trySpecializeVisitableV<'ctx> x |> ValueOption.get

    let trySpecializeAcceptorV<'a> (x: IFolderAcceptor) = 
        let ok, result = FolderTheory.TrySpecialize<'a>(x)
        if ok then ValueSome result else ValueNone

    let specializeAcceptor<'a> x = trySpecializeAcceptorV<'a> x |> ValueOption.get

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