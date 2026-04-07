namespace Nemonuri.Collections.Heterogeneous.Primitives


module Predecessors = begin

    open Unchecked

    let inline private pred<'pred when 'pred :> IPredecessorPremise<'pred> and 'pred : struct> 
        (p: 'pred) = p


    let accept (folder: IFolder<'state>) (acc: 'state) (elem: 'pred) =
        (pred defaultof<'pred>).Accept(folder, acc, elem)

    let length<'pred when 'pred :> IPredecessorPremise<'pred> and 'pred : struct> = defaultof<'pred>.Length


    let visitTail<'tail, 'pred, 'state 
                    when 'tail :> IPredecessorPremise<'tail> and 'tail : struct
                    and 'pred :> IConstructedPredecessorPremise<'tail, 'pred> and 'pred : struct>  
                    folder (acc: 'state) (elem: 'tail) =
        defaultof<'tail>.Accept(folder, acc, elem)

    let tailLength<'tail, 'pred
                    when 'tail :> IPredecessorPremise<'tail> and 'tail : struct
                    and 'pred :> IConstructedPredecessorPremise<'tail, 'pred> and 'pred : struct> =
        defaultof<'tail>.Length
        

end
