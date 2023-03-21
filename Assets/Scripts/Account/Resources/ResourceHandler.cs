namespace Meta.Resources
{
    public abstract class ResourceHandler<T> where T:struct
    {
        #region Public Methods
        public abstract int Stat(T amount);
        public abstract void AddAmount(T amount);
        public abstract void ReduceAmount(T amount);
        public abstract bool HasAmount(T amount);
        #endregion
    }
}
