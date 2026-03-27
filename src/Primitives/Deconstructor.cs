using System.Runtime.InteropServices;
using Nemonuri.Handles;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IDeconstructorPremise<TConsCollection, THeadContext, THead, TTailContext, TTailCollection>
{
    (THead, TTailCollection) Deconstruct(TConsCollection c);
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct DeconstructorHandle<TConsCollection, THeadContext, THead, TTailContext, TTailCollection> : IHandle
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

    public unsafe DeconstructorHandle<TCollection, THeadContext, THead, TTailContext, TTailCollection> UnsafeToUnboxedHandle<THeadContext, THead, TTailContext, TTailCollection>() => 
        new((delegate*<TCollection, (THead, TTailCollection)>)ToIntPtr());
}

public static class DeconstructorTheory
{
    extension<TConsCollection, THeadContext, THead, TTailContext, TTailCollection, TPremise>(TPremise)
        where TPremise : IDeconstructorPremise<TConsCollection, THeadContext, THead, TTailContext, TTailCollection>, new()
    {
        public unsafe static DeconstructorHandle<TConsCollection, THeadContext, THead, TTailContext, TTailCollection> ToHandle()
        {
            static (THead, TTailCollection) Impl(TConsCollection c) => (new TPremise()).Deconstruct(c);

            return new(&Impl);
        }
    }
}
