namespace CardMaga.UI.MetaUI
{
    public class MetaComboUISlot
    {
        private bool _isHaveValue;
        private MetaComboUI _metaComboUI;

        public bool IsHaveValue => _isHaveValue;

        public MetaComboUI MetaComboUI => _metaComboUI;

        public void AssignValue(MetaComboUI metaComboUI)
        {
            if (_isHaveValue)
                return;
            
            _metaComboUI = metaComboUI;
            _isHaveValue = true;
        }

        public void RemoveCombo()
        {
            if (!_isHaveValue)
                return;

            _metaComboUI = null;
            _isHaveValue = false;
        }
    }
}