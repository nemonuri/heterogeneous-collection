namespace Nemonuri.Collections.Heterogeneous.Primitives;


public static class PredecessorTheory
{
    extension<TPredecessor>(TPredecessor)
        where TPredecessor : IPredecessorPremise<TPredecessor>, new()
    {
        public static TState Accept<TState>(IFolder<TState> folder, TState acc, TPredecessor elem) =>
            (new TPredecessor()).Accept(folder, acc, elem);

        public static int GetLength() => (new TPredecessor()).Length;
    }

    extension<TTail, TPredecessor>(TPredecessor)
        where TTail : IPredecessorPremise<TTail>, new()
        where TPredecessor : IConstructedPredecessorPremise<TTail, TPredecessor>, new()
    {
        public static TTail GetTailPremise() => new();

        public static TState VisitTail<TState>(IFolder<TState> folder, TState acc, TTail tailElem) =>
            GetTailPremise<TTail, TPredecessor>().Accept(folder, acc, tailElem);
        
        public static int GetTailLength() => GetTailPremise<TTail, TPredecessor>().Length;
    }
}
