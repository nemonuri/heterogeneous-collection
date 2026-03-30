using System.Runtime.CompilerServices;

namespace Nemonuri.Handles;

internal static class RetypeTheory
{
    public static TTo UnsafeRetype<TFrom, TTo>(scoped ref readonly TFrom from)
    {
        return Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in from));
    }
}
