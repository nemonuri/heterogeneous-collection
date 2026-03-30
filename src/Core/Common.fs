namespace Nemonuri.Collections.Heterogeneous

open System
open System.Runtime.CompilerServices
open Nemonuri.Collections.Heterogeneous.Primitives

#if false
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
#endif


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal BoxedValue =
| Primitive of p: int64
| Object of o: obj

module internal BoxedValues = begin

    let ofTyped (x: 'a) : BoxedValue =
        if typeof<'a>.IsPrimitive && sizeof<'a> <= sizeof<int64> then
            let mutable v : int64 = 0L
            Unsafe.WriteUnaligned(&Unsafe.As<_,_>(&v), x)
            v |> BoxedValue.Primitive
        else
            box x |> BoxedValue.Object
    
    let unsafeToTyped<'a> (item: BoxedValue) : 'a =
        match item with
        | BoxedValue.Primitive p -> 
            let mutable v = p
            Unsafe.ReadUnaligned<'a>(&Unsafe.As<_,_>(&v))
        | BoxedValue.Object o -> unbox o

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