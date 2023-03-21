using CardMaga.Input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ComboAndDeckExitButton : Button
{
    [EventsGroup, SerializeField]
    private UnityEvent OnCardsCollectionOpen;
    [EventsGroup, SerializeField]
    private UnityEvent OnComboCollectionOpen;

    [Header("Collection Reference")]
    [SerializeField] private GameObject _deckCollection;
    [SerializeField] private GameObject _comboCollection;
    [Header("Cycle Collection Button")]
    [SerializeField] private TMP_Text _comboAndDecksButtonText;
    [SerializeField] private ComboAndDeckFilterButton _comboAndDeckFilterButton;

    private InputBehaviour _comboState;
    private InputBehaviour _deckState;

    [SerializeField]
    private bool _isCardStateDefaultState=true;
    protected override void Awake()
    {
        base.Awake();
        _comboState = new InputBehaviour();
        _deckState = new InputBehaviour();
        _comboState.OnClick += SetToCardState;
        _deckState.OnClick += SetToComboState;
        if(_isCardStateDefaultState)
        SetToCardState();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _comboState.OnClick -= SetToCardState;
        _deckState.OnClick -= SetToComboState;
    }

    private void SetToComboState()
    {
        OnComboCollectionOpen?.Invoke();
        _deckCollection.SetActive(false);
        _comboCollection.SetActive(true);
        _comboAndDecksButtonText.text = "Cards";
        TrySetInputBehaviour(_comboState);
        _comboAndDeckFilterButton.SetToComboState();
    }

    private void SetToCardState()
    {
        OnCardsCollectionOpen?.Invoke();
        _deckCollection.SetActive(true);
        _comboCollection.SetActive(false);
        _comboAndDecksButtonText.text = "Combos";
        TrySetInputBehaviour(_deckState);
        _comboAndDeckFilterButton.SetToDeckState();
    }
}
