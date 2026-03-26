namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Collections.Heterogeneous.Primitives

module U = Nemonuri.Collections.Heterogeneous.UntypedItems


[<NoEquality; NoComparison; Struct>]
type HeterogeneousList<'TContext> = internal { Decons: BoxedDeconstructorHandle<'TContext, HeterogeneousList<'TContext>>; Items: UntypedItem list }

module HeterogeneousLists =

    type private Boxed<'ctx> = BoxedDeconstructorHandle<'ctx, HeterogeneousList<'ctx>>
    type private Dth = Nemonuri.Collections.Heterogeneous.Primitives.DeconstructorTheory

    let empty : HeterogeneousList<unit> = { Decons = Boxed<unit>(0); Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        interface IDeconstructorPremise<HeterogeneousList<'hd -> 'tl>, 'hd, HeterogeneousList<'tl>> with
            member _.Deconstruct (c: HeterogeneousList<'hd -> 'tl>): struct ('hd * HeterogeneousList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | hdItem::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl: HeterogeneousList<'tl> = { Decons = c.Decons; Items = tlItems }
                    struct ( hd, tl )

    end


    let cons (hd: 'hd) (tl: HeterogeneousList<'tl>) : HeterogeneousList<'hd -> 'tl> =
        let handle = Dth.ToHandle<HeterogeneousList<'hd -> 'tl>, 'hd, HeterogeneousList<'tl>, Deconstructor<'hd,'tl>>()
        let ni: nativeint = DotNetTypeTheory.UnsafeRetype<_,_>(&handle)
        let hdItem = UntypedItems.ofTyped hd
        { Decons = ni; Items = hdItem::tl.Items }
