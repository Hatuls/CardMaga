using CardMaga.Input;
using TMPro;
using UnityEngine;

namespace Collection
{
    public class CollectionButton : TouchableItem
    {
        [SerializeField] private TMP_Text _comboAndDecksButtonText;

        public TMP_Text ComboAndDecksButtonText => _comboAndDecksButtonText;
    }
}