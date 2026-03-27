namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickUnionItem = { TailDeconsHandle: nativeint }


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type QuickUnion<'TContext> = 
    internal { 
        DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickUnion<'TContext>>; 
        Items: QuickUnionItem list;
        Witness: UntypedItem;
        WitnessedContext: RuntimeTypeHandle
    }


module QuickUnions = begin

    type private I = QuickUnionItem
    type private L<'ctx> = QuickUnion<'ctx>

    let make<'hd,'tl> (hd: 'hd) (_: TypeList<'hd->'tl>) : QuickUnion<'hd->'tl> = 
        {
            DeconsHandle = Unchecked.defaultof<_>;
            Items = [];
            Witness = UntypedItems.ofTyped hd;
            WitnessedContext = TypeHandles.ofType<'hd->'tl>.Value
        }
    
    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<QuickUnion<'hd -> 'tl>, 'hd, unit, 'tl, QuickUnion<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<QuickUnion<'hd -> 'tl>, 'hd, unit, 'tl, QuickUnion<'tl>> with
            member _.Deconstruct (c: QuickUnion<'hd -> 'tl>): struct (unit * QuickUnion<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle }::tlItems -> 
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems; L.Witness = c.Witness; L.WitnessedContext = c.WitnessedContext }
                    struct ( (), tl )
    end

    let extend<'hd,'tl> (tl: QuickUnion<'tl>) : QuickUnion<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr() }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items; Witness = tl.Witness; WitnessedContext = tl.WitnessedContext }

    let witnessIndex (u: QuickUnion<'ctx>) = u.Items |> List.length


    let split (l: QuickUnion<'hd->'tl>) : Result<'hd,QuickUnion<'tl>> =
        if witnessIndex l = 0 then
            UntypedItems.unsafeToTyped l.Witness |> Ok
        else
            let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,unit,'tl,QuickUnion<'tl>>()
            let struct (_, tlc)  = dhnd.Deconstruct(l)
            Error tlc

    module Patterns = begin

        let inline (|Value|Union|) l =
            match split l with
            | Ok v -> Value v
            | Error v -> Union v

    end


end

