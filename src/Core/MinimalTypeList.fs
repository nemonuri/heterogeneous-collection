namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal MinimalTypeListItem = { TailDeconsHandle: nativeint; Item: RuntimeTypeHandle }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type MinimalTypeList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, MinimalTypeList<'TContext>>; Items: MinimalTypeListItem list }

module MinimalTypeLists = begin

    type private I = MinimalTypeListItem
    type private L<'ctx> = MinimalTypeList<'ctx>

    let empty : MinimalTypeList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<MinimalTypeList<'hd -> 'tl>, 'hd, TypeHandle<'hd>, 'tl, MinimalTypeList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<MinimalTypeList<'hd -> 'tl>, 'hd, TypeHandle<'hd>, 'tl, MinimalTypeList<'tl>> with
            member _.Deconstruct (c: MinimalTypeList<'hd -> 'tl>): struct (TypeHandle<'hd> * MinimalTypeList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = TypeHandles.tryToTypedV<'hd> hdItem |> ValueOption.get
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems }
                    struct ( hd, tl )

    end

    let cons<'hd,'tl> (tl: MinimalTypeList<'tl>) : MinimalTypeList<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); I.Item = TypeHandles.ofType<'hd>.Value }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }
    
    let deconsV (l: MinimalTypeList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,TypeHandle<'hd>,'tl,MinimalTypeList<'tl>>()
        let struct (hd, tlc)  = dhnd.Deconstruct(l)
        struct (hd, tlc)

    let decons l =
        match deconsV l with | struct (hd, tl) -> hd, tl

    let length (l: MinimalHList<'ctx>) = l.Items |> List.length

    let isEmpty l = (length l) = 0


end