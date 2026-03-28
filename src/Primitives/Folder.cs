using System.Diagnostics.CodeAnalysis;

namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IFolder<TState>
{
    TState Step<T>(TState acc, T elem);
}

public interface IFolderVisitable {}

public interface IFolderVisitable<TContext> : IFolderVisitable
{
    IFolderAcceptor Acceptor {get;}
}

public interface IFolderAcceptor {}

public interface IFolderAcceptor<T> : IFolderAcceptor
{
    TState Accept<TState>(IFolder<TState> folder, TState acc, T elem);
}


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
