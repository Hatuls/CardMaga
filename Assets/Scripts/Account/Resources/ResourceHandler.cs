namespace Account.Resources
{
    public abstract class ResourceHandler
    {
        #region Public Methods
        public abstract ushort Stat(ushort amount);
        public virtual void AddAmount(ushort amount)
        {

        }
        public virtual void ReduceAmount(ushort amount)
        {

        }
        public virtual void HasAmount(ushort amount)
        {
            
        }
        #endregion
    }
}
