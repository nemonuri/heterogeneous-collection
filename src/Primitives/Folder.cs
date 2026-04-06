namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IFolder<TState>
{
    TState Step<T>(TState acc, T elem);
}

public interface IFolderVisitablePremise<TElement>
{
    TState Accept<TState>(IFolder<TState> folder, TState acc, TElement elem);
}


public interface IPredecessorPremise<TSelf> : IFolderVisitablePremise<TSelf>
    where TSelf : IPredecessorPremise<TSelf>
{
    int Length {get;}
}


public interface IConstructedPredecessorPremise<TTail, TSelf> : IPredecessorPremise<TSelf>
    where TTail : IPredecessorPremise<TTail>
    where TSelf : IConstructedPredecessorPremise<TTail, TSelf>
{}
