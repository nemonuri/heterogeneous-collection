#nowarn "42"

namespace rec Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison>]
type PureHList<'ctx> =
    private
    | Empty
    | Cons of ctx:'ctx * arrowPtr:nativeint * visitable:IFolderVisitable<'ctx>

module PureHLists = begin

    type private vunit = System.ValueTuple

    type private Context<'hd, 'tl> = System.ValueTuple<'hd, PureHList<'tl>>


    [<NoEquality; NoComparison>]
    type private IdentityArrow<'ctx, 'hd, 'tl> = struct

        interface IInArrowPremise<'ctx, Context<'hd, 'tl>> with
            member x.Apply (source: inref<'ctx>): Context<'hd, 'tl> = 
                assert ( typeof<'ctx> = typeof<Context<'hd, 'tl>> );
                RetypeTheory.UnsafeRetype<'ctx,Context<'hd, 'tl>>(&source)

    end

    type private IdentityArrowHandle<'ctx, 'hd, 'tl> = InArrowHandle<'ctx, Context<'hd, 'tl>>

    [<NoEquality; NoComparison>]
    type private Decons<'ctx, 'hd, 'tl> = struct

        static member TryDeconstruct (source: inref<PureHList<'ctx>>, result: byref<Context<'hd, 'tl>>, error: byref<vunit>): bool = 
            match source with
            | PureHList.Empty -> error <- vunit(); false
            | PureHList.Cons (ctx, arrowPtr, _) -> 
                let arrowHandle = HandleTheory.UnsafeFromIntPtr<IdentityArrowHandle<'ctx, 'hd, 'tl>>(arrowPtr) in
                result <- arrowHandle.Apply(&ctx);
                true

        interface IInRefInductivePremise<PureHList<'ctx>, Context<'hd, 'tl>, vunit> with
            member x.TryDeconstruct (source, result, error): bool = Decons<'ctx, 'hd, 'tl>.TryDeconstruct(&source, &result, &error)

    end

    let empty: PureHList<vunit> = PureHList.Empty

    let isEmpty (l: PureHList<_>) = l.IsEmpty

    let tryDeconsV (l: PureHList<_>) =
        let mutable result = Unchecked.defaultof<_>
        let mutable error = Unchecked.defaultof<_>
        let ok = Decons<_,_,_>.TryDeconstruct(&l, &result, &error)
        if ok then ValueSome result else ValueNone
    
    let cons (hd: 'hd) (l: PureHList<'tl>) =
        let arrowPtr = InArrowTheory.ToHandle<_,_,IdentityArrow<Context<'hd, 'tl>, 'hd, 'tl>>().ToIntPtr() in
        PureHList.Cons (struct (hd, l), arrowPtr, PureHLists.Visitable<'hd,'tl>.Instance)
    
    let private fold_core (folder: IFolder<'state>) (acc: 'state) (l: PureHList<'ctx>) =
        match l with
        | PureHList.Cons (ctx, _, visitable) -> visitable.Accept(folder, acc, ctx)
        | _ -> acc
            

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

    let fold folder (seed: 'state) (l: PureHList<'ctx>) = fold_core folder seed l


end