using System.Runtime.CompilerServices;

namespace Nemonuri.Handles;

public interface IInArrowPremise<TSource, TTarget>
{
    TTarget Apply(in TSource source);
}

public unsafe readonly struct InArrowHandle<TSource, TTarget> : IHandle
{
    private readonly delegate*<in TSource, TTarget> _fp;

    internal InArrowHandle(delegate*<in TSource, TTarget> fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public TTarget Apply(in TSource source) => _fp(source);
}

public static class InArrowTheory
{
    extension<TSource, TTarget, TPremise>(TPremise)
        where TPremise : IInArrowPremise<TSource, TTarget>, new()
    {
        public unsafe static InArrowHandle<TSource, TTarget> ToHandle()
        {
            static TTarget Impl(in TSource source) => (new TPremise()).Apply(in source);

            return new(&Impl);
        }
    }
}
