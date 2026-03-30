using System.Runtime.CompilerServices;

namespace Nemonuri.Handles;

public interface IArrowPremise<TSource, TTarget>
{
    TTarget Apply(TSource source);
}

public unsafe readonly struct ArrowHandle<TSource, TTarget> : IHandle
{
    private readonly delegate*<TSource, TTarget> _fp;

    internal ArrowHandle(delegate*<TSource, TTarget> fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public TTarget Apply(TSource source) => _fp(source);
}

public static class ArrowTheory
{
    extension<TSource, TTarget, TPremise>(TPremise)
        where TPremise : IArrowPremise<TSource, TTarget>, new()
    {
        public unsafe static ArrowHandle<TSource, TTarget> ToHandle()
        {
            static TTarget Impl(TSource source) => (new TPremise()).Apply(source);

            return new(&Impl);
        }
    }
}
