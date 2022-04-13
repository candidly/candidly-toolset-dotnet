using System;

namespace Candidly.Collections
{
    internal interface IReadOnlyRefCollectionInternal<T> : IRefEnumerableInternal<T>
    {
        int Count { get; }
    }
}
