using Battle;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Battle.Combo;
using CardMaga.UI.Card;

namespace UI
{

    [System.Serializable]
    public class ComboRecipeEvent : UnityEvent<ComboRecipeUI> { }
    public class ComboRecipeUI : MonoBehaviour
    {

        [BoxGroup("References")]
        [SerializeField]
        CardUI _cardUI;
        [SerializeField]
        Combo _combo;

        [SerializeField]
        Image _innerImage;
        [SerializeField]
        Image _outerImage;
        [SerializeField]
        Image _decorImage;


        [SerializeField]
        ComboRecipeEvent _event;

        [BoxGroup("References")]
        

        [BoxGroup("References")]
        [SerializeField]
        Transform placeHolderContainer;



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
        public Combo Combo { get => _combo; set => _combo = value; }
    

        private void Start()
        {
            activePlaceHolders = 0;

        }
        public void ResetClick() { _event.RemoveAllListeners(); }
        public void RegisterClick(UnityAction<ComboRecipeUI> combo) => _event.AddListener(combo);
        public void RemoveAllFunctionality() => _event.RemoveAllListeners();
        public void RemoveFunctionality(UnityAction<ComboRecipeUI> combo) => _event.RemoveListener(combo);
        public void OnClick()
            => _event?.Invoke(this);
        public void InitRecipe(Combo combo)
        {
            _combo = combo;
         
            var craftedCard = Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(combo.ComboSO.CraftedCard, combo.Level);
            _cardUI.AssingVisual(craftedCard);
            ActivatedPlaceHolders(ComboRecipe);
            SetVisual(ComboRecipe);
            AssignComboCrafting();

        }
        //Redo please
        public void AssignComboCrafting()
        {
          //  var setOfImage = _gotoIconCollection.GetInnerImage(_combo.ComboSO.GoToDeckAfterCrafting);
          //  _innerImage.sprite = setOfImage.Icon;
            //  _innerImage.color = setOfImage.GetMainColor(_combo.ComboSO.CraftedCard.CardTypeEnum);
            //_decorImage.sprite = _gotoIconCollection.GetDecorImage().Icon;
           // _outerImage.sprite = _gotoIconCollection.GetBackGroundImage().Icon;
        }
        private void ActivatedPlaceHolders(ComboSO relicSO)
        {

            if (relicSO.ComboSequence.Length != activePlaceHolders)
            {
                int remain = relicSO.ComboSequence.Length - activePlaceHolders;
                if (remain < 0)
                {
                    for (int i = _placeHolderSlotUIs.Length - 1; i >= relicSO.ComboSequence.Length; i--)
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
            for (int i = 0; i < _placeHolderSlotUIs.Length; i++)
            {
                if (!_placeHolderSlotUIs[i].gameObject.activeSelf)
                    _placeHolderSlotUIs[i].gameObject.SetActive(true);

                if (i < relic.ComboSequence.Length)
                    _placeHolderSlotUIs[i].InitPlaceHolder(relic.ComboSequence[i]);
                
                else
                    _placeHolderSlotUIs[i].ResetSlotUI();
            }
        }
    }

}