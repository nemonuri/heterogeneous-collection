using System.Runtime.InteropServices;
using Nemonuri.Handles;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IDeconstructorPremise<TConsCollection, THeadContext, TTailContext, TTailCollection>
{
    (THeadContext, TTailCollection) Deconstruct(TConsCollection c);
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct DeconstructorHandle<TConsCollection, THead, TTailContext, TTailCollection> : IHandle
{
    private readonly delegate*<TConsCollection, (THead, TTailCollection)> _fp;

    internal DeconstructorHandle(delegate*<TConsCollection, (THead, TTailCollection)> fp)
    {
        _fp = fp;
    }


    /// <remarks>Default value is 0.</remarks>
    public nint ToIntPtr() => (nint)_fp;

    public (THead, TTailCollection) Deconstruct(TConsCollection c) => _fp(c);

    public BoxedDeconstructorHandle<TConsContext, TConsCollection> ToBoxedHandle<TConsContext>() => new(ToIntPtr());
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct BoxedDeconstructorHandle<TContext, TCollection> : IHandle
{
    private readonly nint _fp;

    internal BoxedDeconstructorHandle(nint fp)
    {
        _fp = fp;
    }

    /// <remarks>Default value is 0.</remarks>
    public nint ToIntPtr() => (nint)_fp;

    public unsafe DeconstructorHandle<TCollection, THead, TTailContext, TTailCollection> UnsafeToUnboxedHandle<THead, TTailContext, TTailCollection>() => 
        new((delegate*<TCollection, (THead, TTailCollection)>)ToIntPtr());
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
