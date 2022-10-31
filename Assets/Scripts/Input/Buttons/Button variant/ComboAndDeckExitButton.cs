using CardMaga.Input;
using TMPro;
using UnityEngine;

public class ComboAndDeckExitButton : Button
{
    [Header("Collection Reference")]
    [SerializeField] private GameObject _deckCollection;
    [SerializeField] private GameObject _comboCollection;
    [Header("Cycle Collection Button")]
    [SerializeField] private TMP_Text _comboAndDecksButtonText;
    [SerializeField] private ComboAndDeckFilterButton _comboAndDeckFilterButton;
    
    private InputBehaviour _comboState;
    private InputBehaviour _deckState;

    protected override void Awake()
    {
        base.Awake();
        _comboState = new InputBehaviour();
        _deckState = new InputBehaviour();
        _comboState.OnClick += SetToDeckState;
        _deckState.OnClick += SetToComboState;
        SetToComboState();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _comboState.OnClick -= SetToDeckState;
        _deckState.OnClick -= SetToComboState;
    }

    private void SetToComboState()
    {
        _deckCollection.SetActive(false);
        _comboCollection.SetActive(true);
        _comboAndDecksButtonText.text = "Decks";
        TrySetInputBehaviour(_comboState);
        _comboAndDeckFilterButton.SetToComboState();
    }

    private void SetToDeckState()
    {
        _deckCollection.SetActive(true);
        _comboCollection.SetActive(false);
        _comboAndDecksButtonText.text = "Combo";
        TrySetInputBehaviour(_deckState);
        _comboAndDeckFilterButton.SetToDeckState();
    }
}
