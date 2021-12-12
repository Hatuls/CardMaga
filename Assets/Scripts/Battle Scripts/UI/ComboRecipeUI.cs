using Battles.UI;
using Combo;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace UI
{

    [System.Serializable]
    public class ComboRecipeEvent : UnityEvent<ComboRecipeUI> { }
    public class ComboRecipeUI : MonoBehaviour
    {

        [BoxGroup("References")]
        [SerializeField]
        CardUI _cardUI;

        ComboSO _comboRecipe;

        [SerializeField]
        ComboRecipeEvent _event;

        [BoxGroup("References")]


        [BoxGroup("References")]
        [SerializeField]
        Transform placeHolderContainer;


        [BoxGroup("References")]
        [SerializeField]
        Art.ArtSO _art;


        [BoxGroup("References")]
        [SerializeField]
        Image _backgroundImage;

        [BoxGroup("References")]
        [SerializeField]
        TextMeshProUGUI _recipeTitle;


        [BoxGroup("References")]
        [SerializeField]
        CraftingSlotUI[] _placeHolderSlotUIs;

        byte activePlaceHolders = 0;

        public CardUI CardUI { get => _cardUI; set => _cardUI = value; }
        public ComboSO ComboRecipe { get => _comboRecipe; set => _comboRecipe = value; }

        private void Start()
        {
            activePlaceHolders = 0;

        }

        public void OnClick()
            => _event?.Invoke(this);
        public void InitRecipe(Combo.Combo combo)
        {
            _comboRecipe = combo.ComboSO;
            var craftedCard = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(combo.ComboSO.CraftedCard, combo.Level);
            _cardUI.GFX.SetCardReference(craftedCard);
            ActivatedPlaceHolders(_comboRecipe);
            SetVisual(_comboRecipe);
        }
        public void InitRecipe(ComboSO relicSO)
        {
            if (_comboRecipe != relicSO)
            {
                _comboRecipe = relicSO;
                _cardUI.GFX.SetCardReference(relicSO.CraftedCard);
                ActivatedPlaceHolders(relicSO);
                SetVisual(relicSO);
            }
        }
        private void ActivatedPlaceHolders(ComboSO relicSO)
        {

            if (relicSO.ComboSequance.Length != activePlaceHolders)
            {
                int remain = relicSO.ComboSequance.Length - activePlaceHolders;
                if (remain < 0)
                {
                    for (int i = _placeHolderSlotUIs.Length - 1; i >= relicSO.ComboSequance.Length; i--)
                    {
                        _placeHolderSlotUIs[i].gameObject.SetActive(false);
                        activePlaceHolders--;
                    }
                }
                else
                {
                    for (int i = 0; i < _placeHolderSlotUIs.Length - 1; i++)
                    {
                        _placeHolderSlotUIs[i].gameObject.SetActive(true);
                        activePlaceHolders++;
                    }
                }
            }
        }
        private void SetVisual(ComboSO relic)
        {
            _cardUI.GFX.SetCardReference(relic.CraftedCard);


            for (int i = 0; i < _placeHolderSlotUIs.Length; i++)
            {
                if (!_placeHolderSlotUIs[i].gameObject.activeSelf)
                    _placeHolderSlotUIs[i].gameObject.SetActive(true);

                if (i < relic.ComboSequance.Length)
                    _placeHolderSlotUIs[i].InitPlaceHolder(relic.ComboSequance[i]);
                
                else
                    _placeHolderSlotUIs[i].ResetSlotUI();
                
         
            }
        }
    }

}