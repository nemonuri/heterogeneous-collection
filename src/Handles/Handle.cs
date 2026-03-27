using System.Runtime.CompilerServices;

namespace Nemonuri.Handles;

public interface IHandle
{
    nint ToIntPtr();
}


public static class HandleTheory
{
    public static bool CheckHasValue(nint intPtr)
    {
        return intPtr != 0;
    }

    public static bool CheckHasValue(IHandle? handle)
    {
        if (handle is null) { return false; }
        return CheckHasValue(handle.ToIntPtr());
    }

    public static bool Equals(IHandle? left, IHandle? right)
    {
        if (left == null || right == null) { return false; }
        return left.ToIntPtr() == right.ToIntPtr();
    }

    public static int GetHashCode(IHandle? handle)
    {
        if (handle == null) { return 0; }
        return handle.ToIntPtr().GetHashCode();
    }

    public static bool IsSizeEqualToIntPtr<THandle>()
    {
        return Unsafe.SizeOf<THandle>() == IntPtr.Size;
    }

    private static TTo UnsafeRetype<TFrom, TTo>(scoped ref readonly TFrom from)
    {
        return Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in from));
    }

    public static THandle UnsafeAsHandle<THandle>(nint ptr)
    {
        return UnsafeRetype<nint, THandle>(in ptr);
    }
}
