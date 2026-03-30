using System.Diagnostics.CodeAnalysis;
using Nemonuri.Handles;

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

public interface IInRefInductivePremise<TUnifedSource, TUnifiedTarget, TUnifiedError>
{
    bool TryDeconstruct
    (
        in TUnifedSource source, 
        [NotNullWhen(true)] out TUnifiedTarget? result,
        [NotNullWhen(false)] out TUnifiedError? error
    );
}

public unsafe readonly struct InRefInductiveHandle<TUnifedSource, TUnifiedTarget, TUnifiedError> : IHandle
{
    private readonly delegate*<in TUnifedSource, out TUnifiedTarget, out TUnifiedError, bool> _fp;

    internal InRefInductiveHandle(delegate*<in TUnifedSource, out TUnifiedTarget, out TUnifiedError, bool> fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public bool TryDeconstruct
    (
        in TUnifedSource source, 
        [NotNullWhen(true)] out TUnifiedTarget? result,
        [NotNullWhen(false)] out TUnifiedError? error
    )
        =>
    _fp(in source, out result, out error);
}

public static class InRefInductivePremiseTheory
{
    extension<TUnifedSource, TUnifiedTarget, TUnifiedError, TPremise>(TPremise)
        where TPremise : IInRefInductivePremise<TUnifedSource, TUnifiedTarget, TUnifiedError>, new()
    {
        public unsafe static InRefInductiveHandle<TUnifedSource, TUnifiedTarget, TUnifiedError> ToHandle()
        {
            static bool Impl(in TUnifedSource source, [NotNullWhen(true)] out TUnifiedTarget? result, [NotNullWhen(false)] out TUnifiedError? error) => 
                (new TPremise()).TryDeconstruct(in source, out result, out error);

#pragma warning disable CS8622
            return new(&Impl);
#pragma warning restore CS8622
        }
    }
}


