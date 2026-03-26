using System.Runtime.CompilerServices;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

internal static class DotNetTypeTheory
{
    public static TTo UnsafeRetype<TFrom, TTo>(scoped ref readonly TFrom from)
    {
        return Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in from));
    }
}
