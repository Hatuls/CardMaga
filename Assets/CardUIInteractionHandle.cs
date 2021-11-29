﻿using Battles.UI;
using UnityEngine;

public class CardUIInteractionHandle : MonoBehaviour
{
    [SerializeField]
    CardUI _card;
    [SerializeField]
    CardUIInfoScreen _cardUIInfoScreen;
    [SerializeField]
    DismentalScreen _dismentalScreen;
    [SerializeField]
    GameObject _parent;
    public void Subscribe()
    {
        UI.Meta.Laboratory.MetaCardUIHandler.OnInfoEvent += Open;
        Close();
    }

    public void UnSubscribe()
    {

        UI.Meta.Laboratory.MetaCardUIHandler.OnInfoEvent -= Open;
    }
    public void Open(CardUI card)
    {
          _card = card;
        OpenInfoScreen();
        _parent.SetActive(true);
    }

    public void OpenInfoScreen()
    {
        _parent.SetActive(true);
        _dismentalScreen.gameObject.SetActive(false);
        _cardUIInfoScreen.OpenInfoScreen(_card);
        _cardUIInfoScreen.gameObject.SetActive(true);
        _parent.SetActive(false);
    }

    public void Close()
    {
        _dismentalScreen.gameObject.SetActive(false);
        _cardUIInfoScreen.gameObject.SetActive(false);
        _parent.SetActive(false);
    }

    public void OpenDismentalScreen()
    {
        _cardUIInfoScreen.gameObject.SetActive(false);
        _dismentalScreen.Open(_card);
    }
}
public static class DismentalHandler
{
    static DismentalCostsSO _dismentalCostsSO = Resources.Load<DismentalCostsSO>("MetaGameData/DismentalCostSO");

    public static bool DismentalCard(Cards.Card card)
    {
        if (card == null)
            throw new System.Exception($"DismentalHandler: Card is null");
        var account = Account.AccountManager.Instance;
        if (account.AccountCards.RemoveCard(card.CardCoreInfo.InstanceID))
        {
            account.AccountGeneralData.AccountResourcesData.Chips.AddValue(_dismentalCostsSO.GetCardDismentalCost(card));

            Debug.Log("DismentalCard Successfull!");
            return true;
        }
            Debug.Log("DismentalCard was unSuccessfull!");
        return false;
    }
}