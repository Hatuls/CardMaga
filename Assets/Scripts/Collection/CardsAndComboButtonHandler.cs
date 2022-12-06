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

            [SerializeField] private bool _startInCards;
            [Header("Collection Reference")]
            [SerializeField] private GameObject _deckCollection;
            [SerializeField] private GameObject _comboCollection;

            [Header("Cycle Collection Button")] 
            [SerializeField] private CollectionButton[] _buttons;
            
            private InputBehaviour _comboState;
            private InputBehaviour _cardState;
            
            private void Awake()
            {
                _comboState = new InputBehaviour();
                _cardState = new InputBehaviour();
                _comboState.OnClick += SetToCardState;
                _cardState.OnClick += SetToComboState;
                
                if (_startInCards)
                    SetToCardState();
                else
                    SetToComboState();
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
                
                foreach (var button in _buttons)
                {
                    button.ComboAndDecksButtonText.text = "Cards";
                    button.TrySetInputBehaviour(_comboState);
                }
            }
        
            private void SetToCardState()
            {
                OnCardsCollectionOpen?.Invoke();
                _deckCollection.SetActive(true);
                _comboCollection.SetActive(false);
                
                foreach (var button in _buttons)
                {
                    button.ComboAndDecksButtonText.text = "Combos";
                    button.TrySetInputBehaviour(_cardState);
                }
            }
    }
}