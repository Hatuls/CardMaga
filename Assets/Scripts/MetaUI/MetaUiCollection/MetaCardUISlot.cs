using MetaUI.MetaCardUI;

namespace CardMaga.UI.MetaUI
{
    public class MetaCardUISlot
    {
        private bool _isHaveValue;
        private MetaCardUI _metaCardUI;

        public bool IsHaveValue => _isHaveValue;

        public MetaCardUI MetaCardUI => _metaCardUI;

        public void AssignValue(MetaCardUI metaCardUI)
        {
            if (_isHaveValue)
                return;
            
            _metaCardUI = metaCardUI;
            _isHaveValue = true;
        }

        public void RemoveCard()
        {
            if (!_isHaveValue)
                return;

            _metaCardUI = null;
            _isHaveValue = false;
        }
    }
}