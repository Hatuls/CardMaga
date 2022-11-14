namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseSlot<T> where T : class
    {
        private T _collectionObject;

        public bool IsHaveValue => _collectionObject != null;

        public T CollectionObject => _collectionObject;

        public void AssignValue(T collectionObject)
        {
            if (IsHaveValue)
                return;
            
            _collectionObject = collectionObject;
        }

        public void RemoveValue()
        {
            if (!IsHaveValue)
                return;

            _collectionObject = null;
        }
    }
}