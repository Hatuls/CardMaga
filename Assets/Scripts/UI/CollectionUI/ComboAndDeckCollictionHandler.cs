using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboAndDeckCollictionHandler : MonoBehaviour
{
    [SerializeField] private GameObject _deckCollection;
    [SerializeField] private GameObject _comboCollection;
    [SerializeField] private TMP_Text _comboAndDecksButtonText;
    [SerializeField] private ComboAndDeckButton _comboAndDeckButton;

    private void Start()
    {
        _comboAndDeckButton.ComboState.OnClick += SetToDeckScreen;
        _comboAndDeckButton.DeckState.OnClick += SetToComboScreen;
    }

    private void OnDestroy()
    {
        _comboAndDeckButton.ComboState.OnClick -= SetToDeckScreen;
        _comboAndDeckButton.DeckState.OnClick -= SetToComboScreen;
    }

    private void SetToComboScreen(ButtonGenaric buttonGenaric)
    {
        _deckCollection.SetActive(false);
        _comboCollection.SetActive(true);
        _comboAndDecksButtonText.text = "Decks";
        _comboAndDeckButton.SetToComboState();
    }

    private void SetToDeckScreen(ButtonGenaric buttonGenaric)
    {
        _deckCollection.SetActive(true);
        _comboCollection.SetActive(false);
        _comboAndDecksButtonText.text = "Combo";
        _comboAndDeckButton.SetToDeckState();
    }
}
