namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type TypeList<'TPred> = private | T 


module TypeLists = begin   

    open Unchecked

    [<NoEquality; NoComparison>]
    type Empty = struct

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Empty) = acc

        interface IFolderVisitable<Empty> with
            member _.Accept (folder, acc, elem) = Empty.Accept(folder, acc, elem)

    end

    let empty: TypeList<Empty> = TypeList.T

    let isEmpty (l: TypeList<'a>) = typeof<'a> = typeof<Empty>

    let private toPred<'pred 
                        when 'pred :> IFolderVisitable<'pred>
                        and 'pred : unmanaged> (l: TypeList<'pred>) = defaultof<'pred>

    let private fold_core folder acc (l: TypeList<_>) =
        let pred = toPred l in
        pred.Accept(folder, acc, pred)

    [<NoEquality; NoComparison>]
    type Pair<'hd, 'pred 
                when 'pred : unmanaged
                and 'pred :> IFolderVisitable<'pred>> = struct  //private { TypeList: TypeList<'pred> } 

        member internal x.Tail : TypeList<'pred> = TypeList.T

        static member private Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'pred>) = 
            let newAcc = folder.Step<'hd>(acc, Unchecked.defaultof<_>)
            elem.Tail |> fold_core folder newAcc

        interface IFolderVisitable<Pair<'hd, 'pred>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'pred>.Accept(folder, acc, elem)

    end

    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<_,_>) = pair.Tail

    end


    let private tryPredV (l: TypeList<_>) =
        if isEmpty l then
            ValueNone
        else
            (toPred l) |> ValueSome
    
    let private toPair (l: TypeList<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let tail l = toPair l |> Pairs.tail

    let cons<'hd, 'pred
                when 'pred : unmanaged
                and 'pred :> IFolderVisitable<'pred>> (tl: TypeList<'pred>) : TypeList<Pair<'hd,'pred>> = TypeList.T
    
    let fold folder acc l = fold_core folder acc l

    let private folderForLength = { new IFolder<int> with member _.Step (acc: int, _: 'T): int = acc + 1 }

    let length l = fold folderForLength 0 l

end
