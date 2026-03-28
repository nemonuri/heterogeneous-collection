namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal MinimalHListItem = { TailDeconsHandle: nativeint; Item: UntypedItem }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type MinimalHList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, MinimalHList<'TContext>>; Items: MinimalHListItem list }


module MinimalHLists = begin

    type private I = MinimalHListItem
    type private L<'ctx> = MinimalHList<'ctx>

    let empty : MinimalHList<unit> = { DeconsHandle = Unchecked.defaultof<_> ; Items = [] }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<MinimalHList<'hd -> 'tl>, 'hd, 'hd, 'tl, MinimalHList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<MinimalHList<'hd -> 'tl>, 'hd, 'hd, 'tl, MinimalHList<'tl>> with
            member _.Deconstruct (c: MinimalHList<'hd -> 'tl>): DeconstructResult<'hd, MinimalHList<'tl>> = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem }::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl = { L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); L.Items = tlItems }
                    DeconstructResult<_,_>( hd, tl )

    end

    let cons (hd: 'hd) (tl: MinimalHList<'tl>) : MinimalHList<'hd -> 'tl> =
        let hdItem = { I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); I.Item = UntypedItems.ofTyped hd }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }
    
    let decons (l: MinimalHList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,'hd,'tl,MinimalHList<'tl>>()
        dhnd.Deconstruct(l).Deconstruct()

    let length (l: MinimalHList<'ctx>) = l.Items |> List.length

    let isEmpty l = (length l) = 0



    module Folders = begin

        [<NoEquality; NoComparison; Struct>]
        type FoldEntry<'TState, 'TContext> = { Folder: IFolder<'TState>; State: 'TState; List: MinimalHList<'TContext> }

        let toFoldEntry folder acc l = { Folder = folder; State = acc; List = l }

        let ofFoldEntryV (entry: FoldEntry<_,_>) = struct ( entry.Folder, entry.State, entry.List )

        let (|FoldEntryV|) entry = ofFoldEntryV entry

        let finish<'state> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: MinimalHList<unit>)) : 'state = acc

        let step<'state, 'hd, 'tl> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: MinimalHList<'hd -> 'tl>)) =
            match decons l with
            | ( hd, tl ) ->
                let state = folder.Step (acc, hd)
                toFoldEntry folder state tl

        let inline internal ( ! ) entry = step entry
        
        [<NoEquality; NoComparison>]
        type Premise = struct

            static member Fold e = e |> finish 

            static member Fold e = !e |> finish

        end

        let inline fold folder (seed: 'state) l =
            let inline call (p: ^p) (s: ^s) (e: ^e) = ((^p or ^s) : (static member Fold: _ -> _) e)
            let entry = toFoldEntry folder seed l
            let result: 'state = call Unchecked.defaultof<Premise> seed entry
            result

        type Premise with

            static member Fold e = ! !e |> finish

            static member Fold e = ! ! !e |> finish

            static member Fold e = ! ! ! !e |> finish

            static member Fold e = ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finish

            static member Fold e = ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! !e |> finish

        end

    end

end