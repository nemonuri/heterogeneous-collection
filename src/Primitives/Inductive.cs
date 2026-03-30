using System.Diagnostics.CodeAnalysis;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

#if false
public interface IInductiveList<TContext, TDeconstructor>
{
    bool CanDeconstruct {get;}

    TDeconstructor GetDeconstructor();
}

public interface IInductiveDeconstructor<TConsList, THead, TTailContext, TTailDeconstructor, TTailList>
    where TTailList : IInductiveList<TTailContext, TTailDeconstructor>
{
    (THead, TTailList) Deconstruct(TConsList c);
}
#endif

public interface IInductivePremise<TUnifedSource, TUnifiedTarget, TUnifiedError>
{
    bool TryDeconstruct
    (
        TUnifedSource source, 
        [NotNullWhen(true)] out TUnifiedTarget? result,
        [NotNullWhen(false)] out TUnifiedError? error
    );
}