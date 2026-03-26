using System.Runtime.InteropServices;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IDeconstructorPremise<TContext, TCollection>
{
    (THead, TTail) Deconstruct<THead, TTail>(TCollection collection);
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct DeconstructorHandle<TCollection, THead, TTail>
{
    private readonly delegate*<TCollection, (THead, TTail)> _fp;

    internal DeconstructorHandle(delegate*<TCollection, (THead, TTail)> fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public bool HasValue => ToIntPtr() != 0;
}


public static class DeconstructorTheory
{
    extension<TContext, TCollection, TPremise>(TPremise)
        where TPremise : IDeconstructorPremise<TContext, TCollection>, new()
    {
        public unsafe static DeconstructorHandle<TCollection, THead, TTail> ToHandle<THead, TTail>()
        {
            static (THead, TTail) Impl(TCollection collection) => (new TPremise()).Deconstruct<THead, TTail>(collection);

            return new(&Impl);
        }
    }
}
