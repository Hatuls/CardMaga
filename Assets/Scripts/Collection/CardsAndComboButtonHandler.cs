using CardMaga.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Collection
{
    public class CardsAndComboButtonHandler : MonoBehaviour
    {
         [EventsGroup,SerializeField]
            private UnityEvent OnCardsCollectionOpen;
            [EventsGroup,SerializeField]
            private UnityEvent OnComboCollectionOpen;
            
            [Header("Collection Reference")]
            [SerializeField] private GameObject _deckCollection;
            [SerializeField] private GameObject _comboCollection;

            [Header("Cycle Collection Button")] 
            [SerializeField] private CollectionButton _topButton;
            [SerializeField] private CollectionButton _bottomButton;
            
            private InputBehaviour _comboState;
            private InputBehaviour _cardState;
            
            private void Awake()
            {
                _comboState = new InputBehaviour();
                _cardState = new InputBehaviour();
                _comboState.OnClick += SetToCardState;
                _cardState.OnClick += SetToComboState;
                SetToCardState();
            }
        
            private void OnDestroy()
            {
                _comboState.OnClick -= SetToCardState;
                _cardState.OnClick -= SetToComboState;
            }
        
            private void SetToComboState()
            {
                OnComboCollectionOpen?.Invoke();
                _deckCollection.SetActive(false);
                _comboCollection.SetActive(true);
                _bottomButton.ComboAndDecksButtonText.text = "Cards";
                _bottomButton.TrySetInputBehaviour(_comboState);
                _topButton.ComboAndDecksButtonText.text = "Cards";
                _topButton.TrySetInputBehaviour(_comboState);
            }
        
            private void SetToCardState()
            {
                OnCardsCollectionOpen?.Invoke();
                _deckCollection.SetActive(true);
                _comboCollection.SetActive(false);
                _bottomButton.ComboAndDecksButtonText.text = "Combos";
                _bottomButton.TrySetInputBehaviour(_cardState);
                _topButton.ComboAndDecksButtonText.text = "Combos";
                _topButton.TrySetInputBehaviour(_cardState);
            }
    }
}