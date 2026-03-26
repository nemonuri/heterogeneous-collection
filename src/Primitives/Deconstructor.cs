using System.Runtime.InteropServices;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IDeconstructorPremise<TConsCollection, THead, TTailContext, TTailCollection>
{
    (THead, TTailCollection) Deconstruct(TConsCollection c);
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct DeconstructorHandle<TConsCollection, THead, TTailContext, TTailCollection>
{
    private readonly delegate*<TConsCollection, (THead, TTailCollection)> _fp;

    internal DeconstructorHandle(delegate*<TConsCollection, (THead, TTailCollection)> fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public bool HasValue => ToIntPtr() != 0;

    public (THead, TTailCollection) Deconstruct(TConsCollection c) => _fp(c);

    public BoxedDeconstructorHandle<TTailContext, TTailCollection> ToBoxedHandle() => new(ToIntPtr());
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct BoxedDeconstructorHandle<TContext, TContextedCollection>
{
    private readonly nint _fp;

    internal BoxedDeconstructorHandle(nint fp)
    {
        _fp = fp;
    }

    public nint ToIntPtr() => (nint)_fp;

    public unsafe DeconstructorHandle<TConsCollection, THead, TContext, TContextedCollection> UnsafeToUnboxedHandle<TConsCollection, THead>() => 
        new((delegate*<TConsCollection, (THead, TContextedCollection)>)ToIntPtr());
}


public static class DeconstructorTheory
{
    extension<TConsCollection, THead, TTailContext, TTailCollection, TPremise>(TPremise)
        where TPremise : IDeconstructorPremise<TConsCollection, THead, TTailContext, TTailCollection>, new()
    {
        public unsafe static DeconstructorHandle<TConsCollection, THead, TTailContext, TTailCollection> ToHandle()
        {
            static (THead, TTailCollection) Impl(TConsCollection c) => (new TPremise()).Deconstruct(c);

            return new(&Impl);
        }
    }
}
