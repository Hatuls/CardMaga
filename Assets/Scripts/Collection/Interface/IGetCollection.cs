using System.Collections.Generic;

namespace CardMaga.Collection
{
    public interface IGetCollection<T>
    {
        IEnumerable<T> GetCollection { get; }
    }
}


