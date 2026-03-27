namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal TypeListItem = { TailDeconsHandle: nativeint; Item: RuntimeTypeHandle }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type TypeList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, TypeList<'TContext>>; Items: TypeListItem list }

module TypeLists = begin

    type private I = TypeListItem
    type private L<'ctx> = TypeList<'ctx>

    let empty : TypeList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<TypeList<'hd -> 'tl>, 'hd, TypeHandle<'hd>, 'tl, TypeList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<TypeList<'hd -> 'tl>, 'hd, TypeHandle<'hd>, 'tl, TypeList<'tl>> with
            member _.Deconstruct (c: TypeList<'hd -> 'tl>): struct (TypeHandle<'hd> * TypeList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = TypeHandles.tryToTypedV<'hd> hdItem |> ValueOption.get
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems }
                    struct ( hd, tl )

    end

    let cons<'hd,'tl> (tl: TypeList<'tl>) : TypeList<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); I.Item = TypeHandles.ofType<'hd>.Value }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }
    
    let deconsV (l: TypeList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,TypeHandle<'hd>,'tl,TypeList<'tl>>()
        let struct (hd, tlc)  = dhnd.Deconstruct(l)
        struct (hd, tlc)

    let decons l =
        match deconsV l with | struct (hd, tl) -> hd, tl

    let length (l: QuickList<'ctx>) = l.Items |> List.length

    let isEmpty l = (length l) = 0


end