namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickTypeListItem = { TailDeconsHandle: nativeint; Item: RuntimeTypeHandle }

[<RequireQualifiedAccess>]
[<CompiledName("QuickHeterogeneousTypeList`1")>]
[<NoEquality; NoComparison; Struct>]
type QuickTypeList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickTypeList<'TContext>>; Items: QuickTypeListItem list }

module QuickTypeList = begin

    type private I = QuickTypeListItem
    type private L<'ctx> = QuickTypeList<'ctx>

    let empty : QuickTypeList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<QuickTypeList<'hd -> 'tl>, 'hd, 'tl, QuickTypeList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<QuickTypeList<'hd -> 'tl>, 'hd, 'tl, QuickTypeList<'tl>> with
            member _.Deconstruct (c: QuickTypeList<'hd -> 'tl>): struct ('hd * QuickTypeList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems }
                    struct ( hd, tl )

    end

end