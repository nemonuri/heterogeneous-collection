namespace Nemonuri.Collections.Heterogeneous

open Nemonuri.Handles
open Nemonuri.Collections.Heterogeneous.Primitives

type internal IQuickHListAcceptor = interface end

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type internal QuickHListItem = { TailDeconsHandle: nativeint; Item: UntypedItem; Acceptor: IQuickHListAcceptor }

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type QuickHList<'TContext> = internal { DeconsHandle: BoxedDeconstructorHandle<'TContext, QuickHList<'TContext>>; Items: QuickHListItem list }


module QuickHLists = begin

    type private I = QuickHListItem
    type private L<'ctx> = QuickHList<'ctx>

    type private NullAcceptor = class

        private new() = {}

        static member Instance = NullAcceptor()

        member _.Accept (folder: IFolder<'s>, acc: 's, elem: 'a): 's = acc

        interface IFolderVisitable<unit> with
            member x.Accept(folder, acc, elem) = x.Accept (folder, acc, elem)

    end

    type private ConsAcceptor<'hd,'tl> = class

        private new() = {}

        static member Instance = ConsAcceptor<'hd,'tl>()

        


    end


end