using System.Runtime.CompilerServices;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public static class DotNetTypeTheory
{
    public static TTo UnsafeRetype<TFrom, TTo>(in TFrom from)
    {
        return Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in from));
    }
}
