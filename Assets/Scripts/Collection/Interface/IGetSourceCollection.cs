using System.Collections.Generic;

namespace CardMaga.Collection
{
    public interface IGetSourceCollection<T>
    {
        IEnumerable<T> GetCollection { get; }
    }
}


