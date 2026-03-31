#nowarn "42"

namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type PureHList<'TPred> =
    private
    | Empty
    | Cons of predecessor:'TPred * visitable:IFolderVisitable<'TPred>

module PureHLists = begin

    type private Nil = System.ValueTuple

    type private Pair<'hd, 'tl> = System.ValueTuple<'hd, PureHList<'tl>>


    let empty: PureHList<Nil> = PureHList.Empty

    let isEmpty (l: PureHList<_>) = l.IsEmpty

    let private tryPredV (l: PureHList<_>) =
        match l with
        | PureHList.Empty -> ValueNone
        | PureHList.Cons (pred, _) -> ValueSome pred

    let private toPair (l: PureHList<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred
    
    let head l = match toPair l with | struct (hd, _) -> hd

    let tail l = match toPair l with | struct (_, tl) -> tl
    
    let private fold_core (folder: IFolder<'state>) (acc: 'state) (l: PureHList<'ctx>) =
        match l with
        | PureHList.Cons (ctx, visitable) -> visitable.Accept(folder, acc, ctx)
        | _ -> acc
            

    [<NoEquality; NoComparison; Sealed>]
    type private Visitable<'hd, 'tl> = class

        private new() = {}

        static member Instance : Visitable<'hd, 'tl> = Visitable<_,_>()

        static member Accept<'state> (folder: IFolder<'state>, acc: 'state, ctx: Pair<'hd,'tl>): 'state = 
            let struct (hd, tl) = ctx in
            let nextAcc = folder.Step<'hd>(acc, hd)
            fold_core folder nextAcc tl

        interface IFolderVisitable<Pair<'hd, 'tl>> with
            member _.Accept (folder, acc, ctx) = Visitable<'hd, 'tl>.Accept(folder, acc, ctx)

    end

    let cons (hd: 'hd) (l: PureHList<'tl>) =
        PureHList.Cons (struct (hd, l), Visitable<'hd,'tl>.Instance)

    let fold folder seed l = fold_core folder seed l

    let private folderForLength = { new IFolder<int> with member _.Step (acc: int, _: 'T): int = acc + 1 }

    let length l = fold folderForLength 0 l


end