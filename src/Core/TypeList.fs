namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Collections.Heterogeneous.Primitives


[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type TypeList<'TPred when 'TPred :> IFolderVisitable<'TPred>> = private { Pred: 'TPred }


module TypeLists = begin   

    type Empty() = class

        static member internal Instance = Empty()

        static member Accept (folder: IFolder<'TState>, acc: 'TState, elem: Empty) = acc

        interface IFolderVisitable<Empty> with
            member _.Accept (folder, acc, elem) = Empty.Accept(folder, acc, elem)

    end

    let empty: TypeList<Empty> = { Pred = Empty.Instance }

    let isEmpty (l: TypeList<'a>) = typeof<'a> = typeof<Empty>

    let private fold_core folder acc (l: TypeList<_>) = l.Pred.Accept(folder, acc, l.Pred)

    [<NoEquality; NoComparison>]
    type Pair<'hd, 'pred 
                when 'pred : (new: unit -> 'pred)
                and 'pred :> IFolderVisitable<'pred>>() = class  //private { TypeList: TypeList<'pred> } 

        let typeList: TypeList<'pred> = { TypeList.Pred = new 'pred() }

        member x.TypeList = typeList

        static member internal Instance = Pair<'hd, 'pred>()

        static member Accept (folder: IFolder<'TState>, acc: 'TState, elem: Pair<'hd, 'pred>) = 
            let newAcc = folder.Step<'hd>(acc, Unchecked.defaultof<_>)
            elem.TypeList |> fold_core folder newAcc

        interface IFolderVisitable<Pair<'hd, 'pred>> with
            member _.Accept (folder, acc, elem) = Pair<'hd, 'pred>.Accept(folder, acc, elem)

    end

    module private Pairs = begin

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<_,_>) = pair.TypeList

    end


    let private tryPredV (l: TypeList<_>) =
        if isEmpty l then
            ValueNone
        else
            l.Pred |> ValueSome
    
    let private toPair (l: TypeList<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let tail l = toPair l |> Pairs.tail

    let cons<'hd, 'pred
                when 'pred : (new: unit -> 'pred)
                and 'pred :> IFolderVisitable<'pred>> (tl: TypeList<'pred>) = { TypeList.Pred = Pair<'hd, 'pred>.Instance }
    
    let fold folder acc l = fold_core folder acc l

    let private folderForLength = { new IFolder<int> with member _.Step (acc: int, _: 'T): int = acc + 1 }

    let length l = fold folderForLength 0 l

end
