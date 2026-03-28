namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickHListItem = { TailDeconsHandle: nativeint; TailVisitable: IFolderVisitable; Item: UntypedItem; }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type QuickHList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickHList<'TContext>>; Visitable: IFolderVisitable<'TContext>; Items: QuickHListItem list }


module QuickHLists = begin

    type private I = QuickHListItem
    type private L<'ctx> = QuickHList<'ctx>

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type private HeadInfo<'hd,'tl> = { Item: 'hd; TailVisitable: IFolderVisitable<'tl> }

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<QuickHList<'hd -> 'tl>, 'hd, HeadInfo<'hd,'tl>, 'tl, QuickHList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<QuickHList<'hd -> 'tl>, 'hd, HeadInfo<'hd,'tl>, 'tl, QuickHList<'tl>> with
            member _.Deconstruct (c: QuickHList<'hd -> 'tl>): struct (HeadInfo<'hd,'tl> * QuickHList<'tl>) = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem; TailVisitable = tailVisitable }::tlItems -> 
                    let hd : HeadInfo<_,_> = { Item = UntypedItems.unsafeToTyped<'hd> hdItem; TailVisitable = Folders.specializeVisitable<_> tailVisitable }
                    let tl = 
                        { 
                            L.DeconsHandle = HandleTheory.UnsafeAsHandle<_>(tlHandle); 
                            L.Visitable = hd.TailVisitable; 
                            L.Items = tlItems 
                        }
                    struct ( hd, tl )

    end

    (** We need to define deconstructor, before define constructor. *)

    let private deconsInternalV (l: QuickHList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,HeadInfo<'hd,'tl>,'tl,QuickHList<'tl>>()
        let struct (hd, tlc)  = dhnd.Deconstruct(l)
        struct (hd, tlc)

    let deconsV l = 
        match deconsInternalV l with
        | struct (hd, tl) -> struct (hd.Item, tl)

    type private NullAcceptor = class

        private new() = {}

        static member Instance = NullAcceptor()

        member _.Accept (_: IFolder<'s>, acc: 's, _: QuickHList<unit>): 's = acc

        interface IFolderVisitable<unit> with
            member x.Acceptor = x
        
        interface IFolderAcceptor<QuickHList<unit>> with
            member x.Accept (folder, acc, elem) = x.Accept(folder, acc, elem)

    end

    let empty : QuickHList<unit> = { DeconsHandle = Unchecked.defaultof<_>; Visitable = NullAcceptor.Instance; Items = [] }

    type private ConsAcceptor<'hd,'tl> = class

        private new() = {}

        static member Instance = ConsAcceptor<'hd,'tl>()

        member _.Accept (folder: IFolder<'s>, acc: 's, elem: QuickHList<'hd->'tl>): 's = 
            let struct (hd, tl ) = deconsInternalV elem in
            let nextAcc = folder.Step(acc, hd.Item) in
            hd.TailVisitable.Acceptor 
            |> Folders.specializeAcceptor<QuickHList<'tl>> 
            |> _.Accept(folder, nextAcc, tl)

        interface IFolderVisitable<'hd->'tl> with
            member x.Acceptor = x
        
        interface IFolderAcceptor<QuickHList<'hd->'tl>> with
            member x.Accept (folder, acc, elem) = x.Accept(folder, acc, elem)

    end

    let cons (hd: 'hd) (tl: QuickHList<'tl>) : QuickHList<'hd -> 'tl> =
        let hdItem = 
            { 
                I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); 
                I.Item = UntypedItems.ofTyped hd;
                I.TailVisitable = 
            }
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>()
        { DeconsHandle = deconsHandle; Items = hdItem::tl.Items }


end