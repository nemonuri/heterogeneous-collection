namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickListItem = { TailDeconsHandle: nativeint; Item: UntypedItem }

[<RequireQualifiedAccess>]
[<CompiledName("QuickHeterogeneousList`1")>]
[<NoEquality; NoComparison; Struct>]
type QuickList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickList<'TContext>>; Items: QuickListItem list }


module QuickLists = begin

    type private I = QuickListItem
    type private L<'ctx> = QuickList<'ctx>

    let empty : QuickList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<QuickList<'hd -> 'tl>, 'hd, 'tl, QuickList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<QuickList<'hd -> 'tl>, 'hd, 'tl, QuickList<'tl>> with
            member _.Deconstruct (c: QuickList<'hd -> 'tl>): struct ('hd * QuickList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems }
                    struct ( hd, tl )

    end

    let cons (hd: 'hd) (tl: QuickList<'tl>) : QuickList<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); I.Item = UntypedItems.ofTyped hd }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }
    
    let deconsV (l: QuickList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,'tl,QuickList<'tl>>()
        let struct (hd, tlc)  = dhnd.Deconstruct(l)
        struct (hd, tlc)

    let decons l =
        match deconsV l with | struct (hd, tl) -> hd, tl

    let length (l: QuickList<'ctx>) = l.Items |> List.length

    let isEmpty l = (length l) = 0



    module Folders = begin

        [<NoEquality; NoComparison; Struct>]
        type FoldEntry<'TState, 'TContext> = { Folder: IFolder<'TState>; State: 'TState; List: QuickList<'TContext> }

        let toFoldEntry folder acc l = { Folder = folder; State = acc; List = l }

        let ofFoldEntryV (entry: FoldEntry<_,_>) = struct ( entry.Folder, entry.State, entry.List )

        let (|FoldEntryV|) entry = ofFoldEntryV entry

        let finishFold<'state> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: QuickList<unit>)) : 'state = acc

        let foldOnce<'state, 'hd, 'tl> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: QuickList<'hd -> 'tl>)) =
            match decons l with
            | hd, tl ->
                let state = folder.Fold acc hd
                toFoldEntry folder state tl

        let inline internal ( ! ) entry = foldOnce entry
        
        [<NoEquality; NoComparison>]
        type Premise = struct

            static member Fold e = e |> finishFold 

            static member Fold e = !e |> finishFold

        end

        let inline fold folder (seed: 'state) l =
            let inline call (p: ^p) (s: ^s) (e: ^e) = ((^p or ^s) : (static member Fold: _ -> _) e)
            let entry = toFoldEntry folder seed l
            let result: 'state = call Unchecked.defaultof<Premise> seed entry
            result

        type Premise with

            static member Fold e = ! !e |> finishFold

            static member Fold e = ! ! !e |> finishFold

            static member Fold e = ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finishFold

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finishFold

        end

    end

end