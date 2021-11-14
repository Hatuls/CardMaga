
using Battles.UI;
using DesignPattern;
using System.Collections.Generic;
using UnityEngine;

public class DeckAndCombosScreenUI : MonoBehaviour, IObserver
{
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _comboSelectionPanel;
    [SerializeField] GameObject _cardPanel;
    [SerializeField] GameObject _comboPanel;
    [SerializeField] GameObject _deckSelectionPanel;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] ObserverSO observerSO;
    public void Open()
    {
        observerSO.Notify(this);
        _canvasGroup.blocksRaycasts = true;
        OpenCardCollectionScreen();
        _mainPanel.SetActive(true);
    }

    public void Close()
    {
        observerSO.Notify(null);
        _canvasGroup.blocksRaycasts = false;
        _mainPanel.SetActive(false);
        OpenCardCollectionScreen();
    }

    public void OpenCardCollectionScreen()
    {
        TurnCardCollection(true);
        TurnComboCollection(false);
    }

    public void OpenComboCollectionScreen()
    {
        TurnCardCollection(false);
        TurnComboCollection(true);
    }
    private void TurnComboCollection(bool toActivate)
    {
        _comboSelectionPanel.SetActive(toActivate);
        _comboPanel.SetActive(toActivate);
    }
    private void TurnCardCollection(bool toActivate)
    {
        _deckSelectionPanel.SetActive(toActivate);
        _cardPanel.SetActive(toActivate);
    }

    public void OnNotify(IObserver Myself)
    {
        if ((Object)Myself != this)
            _canvasGroup.blocksRaycasts = false;

    }
}
