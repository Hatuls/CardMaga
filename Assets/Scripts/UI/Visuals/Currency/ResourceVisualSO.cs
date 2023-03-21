using CardMaga.Rewards;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Resource Visual SO", menuName = "ScriptableObjects/UI/Visuals/Resource Visual SO")]

    public class ResourceVisualSO : BaseVisualSO
    {
        public CurrencyType MyCurrencyType = CurrencyType.None;
        public Sprite ResourceIcon;
        public Color ResourceTextColor;
        public bool HasAddResourceButton;
        public override void CheckValidation()
        {
            if (ResourceIcon == null)
                throw new System.Exception(this + "ResourceVisualSO has no Icon");
        }
    }
}