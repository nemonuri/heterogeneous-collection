using System.Diagnostics.CodeAnalysis;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IFolder<TState>
{
    TState Step<T>(TState acc, T elem);
}

public interface IFolderVisitable {}

public interface IFolderVisitable<TElem> : IFolderVisitable
{
    TState Accept<TState>(IFolder<TState> folder, TState acc, TElem elem);
}


public interface IInRefFolder<TState>
{
    TState Step<T>(in TState acc, in T elem);
}

public interface IInRefFolderVisitable {}

public interface IInRefFolderVisitable<TElem> : IInRefFolderVisitable
{
    TState Accept<TState>(IInRefFolder<TState> folder, in TState acc, in TElem elem);
}


#if false
public static class FolderTheory
{
    public static bool TrySpecialize<TContext>
    (
        [NotNullWhen(true)] IFolderVisitable? marker, 
        [NotNullWhen(true)] out IFolderVisitable<TContext>? result
    )
    {
        return (result = marker as IFolderVisitable<TContext>) is not null;
    }

    public static bool TrySpecialize<T>
    (
        [NotNullWhen(true)] IFolderAcceptor? marker, 
        [NotNullWhen(true)] out IFolderAcceptor<T>? result
    )
    {
        return (result = marker as IFolderAcceptor<T>) is not null;
    }
}
#endif