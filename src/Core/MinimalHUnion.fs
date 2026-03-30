namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal MinimalHUnionItem = { TailDeconsHandle: nativeint }


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type MinimalHUnion<'TContext> = 
    internal { 
        DeconsHandle: BoxedDeconstructorHandle<'TContext, MinimalHUnion<'TContext>>; 
        Items: MinimalHUnionItem list;
        Witness: UntypedItem;
        WitnessedContext: RuntimeTypeHandle
    }


module MinimalHUnions = begin

    type private I = MinimalHUnionItem
    type private L<'ctx> = MinimalHUnion<'ctx>

    let make<'hd,'tl> (hd: 'hd) (_: MinimalTypeList<'hd->'tl>) : MinimalHUnion<'hd->'tl> = 
        {
            DeconsHandle = Unchecked.defaultof<_>;
            Items = [];
            Witness = UntypedItems.ofTyped hd;
            WitnessedContext = TypeHandles.ofType<'hd->'tl>.Value
        }
    
    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<MinimalHUnion<'hd -> 'tl>, 'hd, unit, 'tl, MinimalHUnion<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<MinimalHUnion<'hd -> 'tl>, 'hd, unit, 'tl, MinimalHUnion<'tl>> with
            member _.Deconstruct (c: MinimalHUnion<'hd -> 'tl>): System.ValueTuple<unit, MinimalHUnion<'tl>> = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle }::tlItems -> 
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeFromIntPtr<_>(tlHandle); L.Items = tlItems; L.Witness = c.Witness; L.WitnessedContext = c.WitnessedContext }
                    System.ValueTuple<_,_>( (), tl )
    end

    let extend<'hd,'tl> (tl: MinimalHUnion<'tl>) : MinimalHUnion<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr() }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items; Witness = tl.Witness; WitnessedContext = tl.WitnessedContext }

    let witnessIndex (u: MinimalHUnion<'ctx>) = u.Items |> List.length


    let split (l: MinimalHUnion<'hd->'tl>) : Result<'hd,MinimalHUnion<'tl>> =
        if witnessIndex l = 0 then
            UntypedItems.unsafeToTyped l.Witness |> Ok
        else
            let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,unit,'tl,MinimalHUnion<'tl>>()
            let struct ( _, tlc) = dhnd.Deconstruct(l)
            Error tlc

    module Patterns = begin

        let inline (|Value|Union|) l =
            match split l with
            | Ok v -> Value v
            | Error v -> Union v

    end


end

