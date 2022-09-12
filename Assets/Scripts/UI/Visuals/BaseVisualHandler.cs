using System;

namespace CardMaga.UI
{
    public abstract class BaseVisualHandler<T> : IDisposable
    {
        public abstract void Init(T data);
        public abstract void CheckValidation();
        public abstract void Dispose();
    }
}
