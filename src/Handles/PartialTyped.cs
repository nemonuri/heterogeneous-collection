namespace Nemonuri.Handles;

public readonly struct PartialTypedHandle<T1>(nint fp) : IHandle
{
    private readonly nint _fp = fp;

    public nint ToIntPtr() => _fp;
}


public readonly struct PartialTypedHandle<T1, T2>(nint fp) : IHandle
{
    private readonly nint _fp = fp;

    public nint ToIntPtr() => _fp;
}

public static class PartialTypedHandleTheory
{
    public static PartialTypedHandle<T1> FromIntPtr<T1>(nint n) => new(n);

    public static PartialTypedHandle<T1, T2> FromIntPtr<T1, T2>(nint n) => new(n);
}
