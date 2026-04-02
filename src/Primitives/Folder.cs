namespace Nemonuri.Collections.Heterogeneous.Primitives;

public interface IFolder<TState>
{
    TState Step<T>(TState acc, T elem);
}

public interface IFolderVisitable<TElem>
{
    TState Accept<TState>(IFolder<TState> folder, TState acc, TElem elem);
}
