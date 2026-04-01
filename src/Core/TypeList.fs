namespace Nemonuri.Collections.Heterogeneous

open System
open Nemonuri.Collections.Heterogeneous.Primitives

#if false
module private TypeListBasics = begin


    type IPredecessor<'TTail> = interface

        abstract member GetHead: unit -> Type

        abstract member GetTail: unit -> 'TTail

    end


    type ISingletonProxy<'T> = interface

        abstract member GetSingleton: unit -> 'T

    end


end


module B = TypeListBasics
#endif

[<RequireQualifiedAccess>]
[<NoEquality; NoComparison; Struct>]
type TypeList<'TPred> = private { Pred: B.ISingletonProxy<'TPred> }


module TypeLists = begin

    type private Nil = System.ValueTuple

    [<RequireQualifiedAccess>]
    [<NoEquality; NoComparison; Struct>]
    type Pair<'hd, 'pred> = private { TypeList: TypeList<'pred> }

    module private Pairs = begin

        let toPair<'hd, 'pred> (tl: TypeList<'pred>) = { Pair.TypeList = tl }

        let head (pair: Pair<'hd,_>) = typeof<'hd>

        let tail (pair: Pair<_,_>) = pair.TypeList

    end


    [<NoEquality; NoComparison>]
    type private NilProxy = struct

        static member Instance : B.ISingletonProxy<Nil> = NilProxy()

        static member NilSingleton = Nil()

        interface B.ISingletonProxy<Nil> with

            member _.GetSingleton () = NilProxy.NilSingleton

    end

    let empty = { TypeList.Pred = NilProxy.Instance }

    let isEmpty (l: TypeList<'a>) = typeof<'a> = typeof<Nil>

    let private tryPredV (l: TypeList<_>) =
        if isEmpty l then
            ValueNone
        else
            l.Pred.GetSingleton() |> ValueSome
    
    let private toPair (l: TypeList<Pair<'hd, 'tl>>) =
        match tryPredV l with
        | ValueNone -> failwith "Unreachable"
        | ValueSome pred -> pred

    let head l = toPair l |> Pairs.head

    let tail l = toPair l |> Pairs.tail

#if false
    [<NoEquality; NoComparison>]
    type private TypeListProxy<'TPred, 'TPredProxy
                                when 'TPredProxy :> B.ISingletonProxy<'TPred> 
                                and 'TPredProxy : unmanaged > = struct

        static member Instance : TypeList<'TPred> = { Pred = Unchecked.defaultof<'TPredProxy> }

        interface B.ISingletonProxy<TypeList<'TPred>> with
            member _.GetSingleton () = TypeListProxy<'TPred, 'TPredProxy>.Instance

    end

    [<NoEquality; NoComparison>]
    type private PairProxy<'THead, 'TPred, 'TPredProxy
                            when 'TPredProxy :> B.ISingletonProxy<'TPred>
                            and 'TPredProxy : unmanaged > = struct

        static member Instance : B.ISingletonProxy<Pair<'THead, 'TPred>> = PairProxy<'THead, 'TPred, 'TPredProxy>()

        static member PairSingleton : Pair<'THead, 'TPred> = TypeListProxy<'TPred, 'TPredProxy>.Instance |> Pairs.toPair<'THead,_>


        interface B.ISingletonProxy<Pair<'THead, 'TPred>> with

            member _.GetSingleton (): Pair<'THead,'TPred> = PairProxy<'THead, 'TPred, 'TPredProxy>.PairSingleton
                    

    end
#endif

    [<NoEquality; NoComparison>]
    type private PairProxy<'THead, 'TPred when 'TPred : (new:unit -> 'TPred)> = struct

        static member Instance : B.ISingletonProxy<Pair<'THead, 'TPred>> = PairProxy<'THead, 'TPred>()

        interface B.ISingletonProxy<Pair<'THead, 'TPred>> with

            member _.GetSingleton (): Pair<'THead,'TPred> = PairProxy<'THead, 'TPred, 'TPredProxy>.PairSingleton

    end


    let cons<'hd, 'pred> (tl: TypeList<'pred>) : TypeList<Pair<'hd, 'pred>> =
        let pair = Pairs.toPair<'hd, 'pred> tl



end


