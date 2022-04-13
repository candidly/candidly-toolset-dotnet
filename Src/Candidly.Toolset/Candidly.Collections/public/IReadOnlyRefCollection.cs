using System;

namespace Candidly.Collections
{
    public interface IReadOnlyRefCollection<T> : IRefEnumerable<T>
    {
        int Count { get; }
    }
}
