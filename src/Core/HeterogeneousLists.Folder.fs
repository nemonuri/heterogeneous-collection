namespace Nemonuri.Collections.Heterogeneous.HeterogeneousLists

open type System.TupleExtensions
open Nemonuri.Collections.Heterogeneous
open Nemonuri.Collections.Heterogeneous.HeterogeneousLists.Operations

module Folders =

    [<NoEquality; NoComparison; Struct>]
    type FoldEntry<'TState, 'TContext> = { Folder: IFolder<'TState>; State: 'TState; HeterogeneousList: HeterogeneousList<'TContext> }

    let toFoldEntry folder acc l = { Folder = folder; State = acc; HeterogeneousList = l }

    let ofFoldEntryV (entry: FoldEntry<_,_>) = struct ( entry.Folder, entry.State, entry.HeterogeneousList )

    let (|FoldEntryV|) entry = ofFoldEntryV entry

    let finishFold<'state> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: HeterogeneousList<unit>)) : 'state = acc

    let foldOnce<'state, 'hd, 'tl> (FoldEntryV struct ( folder: IFolder<'state>, acc: 'state, l: HeterogeneousList<'hd -> 'tl>)) =
        match decons l with
        | hd, tl ->
            let state = folder.Fold acc hd
            toFoldEntry folder state tl

    let inline internal ( ! ) entry = foldOnce entry
    
    [<NoEquality; NoComparison>]
    type QuickPremise = struct

        static member Fold e = e |> finishFold 

        static member Fold e = !e |> finishFold

    end

    let inline foldQuick folder (seed: 'state) l =
        let inline call (p: ^p) (s: ^s) (e: ^e) = ((^p or ^s) : (static member Fold: _ -> _) e)
        let entry = toFoldEntry folder seed l
        let result: 'state = call Unchecked.defaultof<QuickPremise> seed entry
        result

    type QuickPremise with

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
