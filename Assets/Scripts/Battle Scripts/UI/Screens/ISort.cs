using System.Collections.Generic;

namespace Rei.Utilities
{
   
    public interface ISort<T> where T : class
    {
        IEnumerable<T> Sort();
    }
}