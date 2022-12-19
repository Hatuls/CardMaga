using CardMaga.Input;
using TMPro;
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
        [SerializeField] private bool _startInCard;
            [Header("Collection Reference")]
            [SerializeField] private GameObject _deckCollection;
            [SerializeField] private GameObject _comboCollection;

            [Header("Cycle Collection Button")] 
            [SerializeField] private Button[] _buttons;

            [SerializeField] private TMP_Text _title;
            
            private InputBehaviour _comboState;
            private InputBehaviour _cardState;
            
            private void Awake()
            {
                _comboState = new InputBehaviour();
                _cardState = new InputBehaviour();
                _comboState.OnClick += SetToCardState;
                _cardState.OnClick += SetToComboState;

                if (_startInCard)
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
                _title.text = "Combos";
                
                foreach (var button in _buttons)
                {
                    button.SetButtonText("Cards");
                    button.TrySetInputBehaviour(_comboState);
                }
            }
        
            private void SetToCardState()
            {
                OnCardsCollectionOpen?.Invoke();
                _deckCollection.SetActive(true);
                _comboCollection.SetActive(false);
                _title.text = "Cards";
                
                foreach (var button in _buttons)
                {
                    button.SetButtonText("Combos");
                    button.TrySetInputBehaviour(_cardState);
                }
            }
    }
}