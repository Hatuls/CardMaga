using Battles.UI;
using System;
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
    GameObject _container;
    [SerializeField]
    GameObject _parent;

    [SerializeField]
    GameObject _useBtnGO;
    [SerializeField]
    GameObject _dismentalBtnGO;
    [SerializeField]
    GameObject _upgradeBtn;







    // public Action<CardUI> OnRemoveEvent;
    public Action<CardUI> OnDismentalEvent;
    public Action<CardUI> OnUpgradeEvent;
    public Action<CardUI> OnUseEvent;
    // public Action<CardUI> OnInfoEvent;
    //  public Action<CardUI> OnBuyEvent;


    MetaCardUiInteractionEnum _state;
    public void Subscribe()
    {
        //   UI.Meta.Laboratory.MetaCardUIHandler.OnInfoEvent += Open;
        Close();
    }

    public void UnSubscribe()
    {

        // UI.Meta.Laboratory.MetaCardUIHandler.OnInfoEvent -= Open;
    }
    public void Open(CardUI card)
    {
        _card = card;
        OpenInfoScreen();
    }
    public void SetClickFunctionality(MetaCardUiInteractionEnum metaCardUiInteractionEnum, Action<CardUI> action = null)
    {
        switch (metaCardUiInteractionEnum)
        {


            case MetaCardUiInteractionEnum.Use:
                if (action != null)
                    OnUseEvent += action;
                break;
            case MetaCardUiInteractionEnum.Dismental:
                if (action != null)
                    OnDismentalEvent += action;
                break;
            case MetaCardUiInteractionEnum.Upgrade:
                if (action != null)
                    OnUpgradeEvent += action;
                break;
            case MetaCardUiInteractionEnum.None:
            default:
                ResetEnum();
                ClosePanel();
      
                return;
        }

        _state &= ~MetaCardUiInteractionEnum.None;
        _state |= metaCardUiInteractionEnum;
    }
    public void ResetInteraction()
    {
        OnDismentalEvent = null;
        OnUseEvent = null;
        OnUpgradeEvent = null;
    }
    public void ResetEnum()
    {
        _state = MetaCardUiInteractionEnum.None;
        ResetInteraction();
    }
    public void ClosePanel()
         => _container.SetActive(false);
    public void OpenInteractionPanel()
    {
        if (_state != MetaCardUiInteractionEnum.None && !_container.activeSelf)
        {

            bool toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Use);
            _useBtnGO?.SetActive(toOpen);
            toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Upgrade);
            _upgradeBtn?.SetActive(toOpen);
            toOpen = _state.HasFlag(MetaCardUiInteractionEnum.Dismental);
            _dismentalBtnGO?.SetActive(toOpen);
            _container.SetActive(true);
        }
        else
        {
            ClosePanel();
        }
    }


    public void OnUseSelect()
    {
        OnUseEvent?.Invoke(_card);
        Close();
    }

    public void OnDismentalSelect()
    {
        OnDismentalEvent?.Invoke(_card);
        Close();
    }

    public void OnUpgradeSelect()
    {
        OnUpgradeEvent?.Invoke(_card);
        Close();
    }



    public void OpenInfoScreen()
    {
        _parent.SetActive(true);
        _dismentalScreen.gameObject.SetActive(false);
        _cardUIInfoScreen.OpenInfoScreen(_card);
        _cardUIInfoScreen.gameObject.SetActive(true);

    }

    public void Close()
    {
        _dismentalScreen.gameObject.SetActive(false);
        _cardUIInfoScreen.gameObject.SetActive(false);
        _parent.SetActive(false);
    }
    public void OpenDismentalScreen(CardUI card)
    {
        _cardUIInfoScreen.gameObject.SetActive(false);
        _dismentalScreen.Open(card);
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
            SendAnalyticEvents(card);
            account.AccountGeneralData.AccountResourcesData.Chips.AddValue(_dismentalCostsSO.GetCardDismentalCost(card));

            return true;
        }
        return false;
    }

    private static void SendAnalyticEvents(Cards.Card card)
    {
        const string EventName = "dismentaling_card";
         string cardName = card.CardSO.CardName;
        AnalyticsHandler.SendEvent(string.Concat(EventName, cardName));
        FireBaseHandler.SendEvent(EventName, new Firebase.Analytics.Parameter("card_name", cardName));
    }
}