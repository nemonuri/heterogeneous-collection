namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickHListItem = { TailDeconsHandle: nativeint; TailAcceptor: IFolderAcceptor; Item: UntypedItem; }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type QuickHList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickHList<'TContext>>; Acceptor: IFolderAcceptor<QuickHList<'TContext>>; Items: QuickHListItem list }


module QuickHLists = begin

    type private I = QuickHListItem
    type private L<'ctx> = QuickHList<'ctx>

    [<NoEquality; NoComparison>]
    type private Deconstructor<'hd,'tl> = struct

        static member ToHandle() = DeconstructorTheory.ToHandle<QuickHList<'hd -> 'tl>, 'hd, 'hd, 'tl, QuickHList<'tl>, Deconstructor<'hd,'tl>>()

        interface IDeconstructorPremise<QuickHList<'hd -> 'tl>, 'hd, 'hd, 'tl, QuickHList<'tl>> with
            member _.Deconstruct (c: QuickHList<'hd -> 'tl>): System.ValueTuple<'hd, QuickHList<'tl>> = 
                match c.Items with
                | [] -> failwith "Unreachable"
                | { TailDeconsHandle = tlHandle; Item = hdItem; TailAcceptor = tailAcceptor }::tlItems -> 
                    let hd = UntypedItems.unsafeToTyped<'hd> hdItem
                    let tl = 
                        { 
                            L.DeconsHandle = HandleTheory.UnsafeFromIntPtr<_>(tlHandle); 
                            L.Acceptor = tailAcceptor |> unbox; // Folders.specializeAcceptor<_>
                            L.Items = tlItems 
                        }
                    System.ValueTuple<_,_>( hd, tl )

    end

    (** We need to define deconstructor, before define constructor. *)

    let deconsV (l: QuickHList<'hd -> 'tl>) =
        let dhnd = l.DeconsHandle.UnsafeToUnboxedHandle<'hd,'hd,'tl,QuickHList<'tl>>()
        dhnd.Deconstruct(l)


    [<NoEquality; NoComparison; Sealed>]
    type private NullAcceptor = class

        private new() = {}

        static member Instance = NullAcceptor()

        static member Accept (_: IFolder<'s>, acc: 's, _: QuickHList<unit>): 's = acc
        
        interface IFolderAcceptor<QuickHList<unit>> with
            member _.Accept (folder, acc, elem) = NullAcceptor.Accept(folder, acc, elem)

    end

    let empty : QuickHList<unit> = { DeconsHandle = Unchecked.defaultof<_>; Acceptor = NullAcceptor.Instance; Items = [] }

    let length (l: QuickHList<'ctx>) = l.Items |> List.length

    let isEmpty (l: QuickHList<'ctx>) = l.Items |> List.isEmpty

    let private visit (folder: IFolder<'s>) (acc: 's) (l: QuickHList<'ctx>) : 's =
        l.Acceptor.Accept(folder, acc, l)

    [<NoEquality; NoComparison; Sealed>]
    type private ConsAcceptor<'hd,'tl> = class

        private new() = {}

        static member Instance = ConsAcceptor<'hd,'tl>()

        static member Accept (folder: IFolder<'s>, acc: 's, elem: QuickHList<'hd->'tl>): 's = 
            let struct (hd, tl ) = deconsV elem in
            let nextAcc = folder.Step(acc, hd) in
            visit folder nextAcc tl
        
        interface IFolderAcceptor<QuickHList<'hd->'tl>> with
            member x.Accept (folder, acc, elem) = ConsAcceptor<'hd,'tl>.Accept(folder, acc, elem)

    end

    let cons (hd: 'hd) (tl: QuickHList<'tl>) : QuickHList<'hd -> 'tl> =
        let hdItem = 
            { 
                I.TailDeconsHandle = tl.DeconsHandle.ToIntPtr(); 
                I.TailAcceptor = tl.Acceptor;
                I.Item = UntypedItems.ofTyped hd
            }
        in
        let deconsHandle = Deconstructor<'hd,'tl>.ToHandle().ToBoxedHandle<'hd -> 'tl>() in
        let visitable = ConsAcceptor<'hd,'tl>.Instance in
        { DeconsHandle = deconsHandle; Acceptor = visitable; Items = hdItem::tl.Items }

    (* fold is starter of visit. *)
    let fold (folder: IFolder<'state>) (seed: 'state) (l: QuickHList<'ctx>) = visit folder seed l

end