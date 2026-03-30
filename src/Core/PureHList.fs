#nowarn "42"

namespace rec Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type PureHList<'ctx> =
    private
    | Empty
    | Cons of ctx:'ctx * arrowPtr:nativeint * visitable:PureHLists.IFolderVisitable<'ctx>

module PureHLists = begin

    type private vunit = System.ValueTuple

    type private Context<'hd, 'tl> = System.ValueTuple<'hd, PureHList<'tl>>

    let inline private retype<'T,'U> (x:'T) : 'U = (# "" x : 'U #)

    [<NoEquality; NoComparison>]
    type private IdentityArrow<'ctx, 'hd, 'tl> = struct

        interface IArrowPremise<'ctx, Context<'hd, 'tl>> with
            member x.Apply (source: 'ctx): Context<'hd, 'tl> = 
                assert ( typeof<'ctx> = typeof<Context<'hd, 'tl>> );
                source |> retype

    end

    type private IdentityArrowHandle<'ctx, 'hd, 'tl> = ArrowHandle<'ctx, Context<'hd, 'tl>>

    [<NoEquality; NoComparison>]
    type private Decons<'ctx, 'hd, 'tl> = struct

        member x.TryDeconstruct (source: PureHList<'ctx>, result: byref<Context<'hd, 'tl>>, error: byref<vunit>): bool = 
            match source with
            | PureHList.Empty -> error <- vunit(); false
            | PureHList.Cons (ctx, arrowPtr, _) -> 
                let arrowHandle = HandleTheory.UnsafeFromIntPtr<IdentityArrowHandle<'ctx, 'hd, 'tl>>(arrowPtr) in
                result <- arrowHandle.Apply(ctx);
                true

        interface IInductivePremise<PureHList<'ctx>, Context<'hd, 'tl>, vunit> with
            member x.TryDeconstruct (source, result, error): bool = 
                x.TryDeconstruct(source, &result, &error)

    end


    let empty: PureHList<vunit> = PureHList.Empty

    let isEmpty l = 
        match l with
        | PureHList.Empty -> true
        | _ -> false

    let tryDecons l =
        let mutable result = Unchecked.defaultof<_>
        let mutable error = Unchecked.defaultof<_>
        let ok = Decons<_,_,_>().TryDeconstruct(l, &result, &error)
        if ok then ValueSome result else ValueNone
    
    let cons (hd: 'hd) (l: PureHList<'tl>) =
        let ctx = struct (hd, l) in
        let arrowPtr = ArrowTheory.ToHandle<_,_,IdentityArrow<Context<'hd, 'tl>, 'hd, 'tl>>().ToIntPtr() in
        PureHList.Cons (ctx, arrowPtr, PureHLists.Visitable<'hd,'tl>.Instance)
    
    let private fold_core (folder: IFolder<'state>) (acc: 'state) (l: PureHList<'ctx>) =
        match l with
        | PureHList.Cons (ctx, _, visitable) -> visitable.Accept(folder, acc, ctx)
        | _ -> acc
            
    
    type internal IFolderVisitable<'ctx> = interface

        abstract member Accept<'state> : folder: IFolder<'state> * acc: 'state * ctx: 'ctx -> 'state

    end

    type private Visitable<'hd, 'tl> = class

        private new() = {}

        static member Instance : Visitable<'hd, 'tl> = Visitable<_,_>()

        static member Accept<'state> (folder: IFolder<'state>, acc: 'state, ctx: Context<'hd,'tl>): 'state = 
            let struct (hd, tl) = ctx in
            let nextAcc = folder.Step<'hd>(acc, hd)
            fold_core folder nextAcc tl

        interface IFolderVisitable<Context<'hd, 'tl>> with
            member _.Accept (folder, acc, ctx) = Visitable<'hd, 'tl>.Accept(folder, acc, ctx)

    end

    let fold folder acc l = fold_core folder acc l


end