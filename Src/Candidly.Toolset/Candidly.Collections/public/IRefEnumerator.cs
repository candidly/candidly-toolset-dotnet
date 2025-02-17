using System;

namespace Candidly.Collections
{
    /// <summary>
    /// Supports <see cref="IRefEnumerable{T}" />.
    /// </summary>
    public interface IRefEnumerator<T> : IDisposable
    {
        ref T Current { get; }
        bool MoveNext();
        void Reset();
    }
}