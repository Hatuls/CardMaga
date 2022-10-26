using CardMaga.Input;
using TMPro;
using UnityEngine;

public class ComboAndDeckExitButton : ButtonGenaric
{
    [Header("Collection Reference")]
    [SerializeField] private GameObject _deckCollection;
    [SerializeField] private GameObject _comboCollection;
    [Header("Cycle Collection Button")]
    [SerializeField] private TMP_Text _comboAndDecksButtonText;
    [SerializeField] private ComboAndDeckFilterButton _comboAndDeckFilterButton;
    
    private InputBehaviour<ButtonGenaric> _comboState;
    private InputBehaviour<ButtonGenaric> _deckState;

    protected override void Awake()
    {
        base.Awake();
        _comboState = new InputBehaviour<ButtonGenaric>();
        _deckState = new InputBehaviour<ButtonGenaric>();
        _comboState.OnClick += SetToDeckState;
        _deckState.OnClick += SetToComboState;
        SetToComboState(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _comboState.OnClick -= SetToDeckState;
        _deckState.OnClick -= SetToComboState;
    }

    private void SetToComboState(ButtonGenaric buttonGenaric)
    {
        _deckCollection.SetActive(false);
        _comboCollection.SetActive(true);
        _comboAndDecksButtonText.text = "Decks";
        TrySetInputBehaviour(_comboState);
        _comboAndDeckFilterButton.SetToComboState();
    }

    private void SetToDeckState(ButtonGenaric buttonGenaric)
    {
        _deckCollection.SetActive(true);
        _comboCollection.SetActive(false);
        _comboAndDecksButtonText.text = "Combo";
        TrySetInputBehaviour(_deckState);
        _comboAndDeckFilterButton.SetToDeckState();
    }
}
