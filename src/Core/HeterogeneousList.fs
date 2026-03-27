namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives


[<NoEquality; NoComparison; Struct>]
type internal HeterogeneousListItem = { TailDeconsHandle: nativeint; Item: UntypedItem }

[<NoEquality; NoComparison; Struct>]
type HeterogeneousList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, HeterogeneousList<'TContext>>; Items: HeterogeneousListItem list }

module HeterogeneousLists =

    let empty : HeterogeneousList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<HeterogeneousList<'hd -> 'tl>, 'hd, 'tl, HeterogeneousList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<HeterogeneousList<'hd -> 'tl>, 'hd, 'tl, HeterogeneousList<'tl>> with
            member _.Deconstruct (c: HeterogeneousList<'hd -> 'tl>): struct ('hd * HeterogeneousList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl = { DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); Items = tlItems }
                    struct ( hd, tl )

    end

    let cons (hd: 'hd) (tl: HeterogeneousList<'tl>) : HeterogeneousList<'hd -> 'tl> =
        let hdItem = { TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); Item = UntypedItems.ofTyped hd }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }
    
    let decons (l: HeterogeneousList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,'tl,HeterogeneousList<'tl>>()
        let struct (hd, tlc)  = dhnd.Deconstruct(l)
        hd, tlc

    let length (l: HeterogeneousList<'ctx>) = l.Items |> List.length

    let isEmpty l = (length l) = 0
