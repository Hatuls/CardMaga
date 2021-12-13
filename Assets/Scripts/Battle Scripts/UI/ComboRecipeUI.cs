using Battles.UI;
using Combo;
using Sirenix.OdinInspector;
using System;
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
        Combo.Combo _combo;
        

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
        public ComboSO ComboRecipe { get => _combo.ComboSO; }
        public Combo.Combo Combo { get => _combo; set => _combo = value; }
    

        private void Start()
        {
            activePlaceHolders = 0;

        }
        public void ResetClick() { _event.RemoveAllListeners(); }
        public void RegisterClick(UnityAction<ComboRecipeUI> combo) => _event.AddListener(combo);
        public void OnClick()
            => _event?.Invoke(this);
        public void InitRecipe(Combo.Combo combo)
        {
            _combo = combo;
         
            var craftedCard = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(combo.ComboSO.CraftedCard, combo.Level);
            _cardUI.GFX.SetCardReference(craftedCard);
            ActivatedPlaceHolders(ComboRecipe);
            SetVisual(ComboRecipe);
        }
        public void InitRecipe(ComboSO relicSO)
        {

      var combo =      Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(relicSO, 0);
            if (ComboRecipe != relicSO)
            {
                _combo  = combo;
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