using System;
using CardMaga.MetaData.Dismantle;
using CardMaga.MetaUI;
using CardMaga.MetaUI.DismantelUI;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using UnityEngine;

public class DismantelUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
{
    [SerializeField] private DismantleDataManager _dismantleDataManager;
    [SerializeField] private MetaCollectionUIHandler _collectionHandler;
    [SerializeField] private DismantelCurrencyUIHandler _dismantelCurrencyUI;
    
    public int Priority => 0;

    public DismantelCurrencyUIHandler DismantelCurrencyUI => _dismantelCurrencyUI;

    public DismantleDataManager DismantleDataManager => _dismantleDataManager; 

    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        _collectionHandler.Init();
        _dismantleDataManager = data.MetaDataManager.DismantleDataManager;

        _dismantleDataManager.OnCardAddToDismantel += UpdateVisual;
    }

    public override void OpenScreen()
    {
        base.OpenScreen();
        UpdateVisual(0, 0);
        _dismantleDataManager.SetCardCollection();
        _collectionHandler.LoadObjects(VisualRequesterManager.Instance.GetMetaCollectionWithoutLimitCardUI(_dismantleDataManager.CardCollectionDatas.CollectionCardDatas),null);
    }

    private void UpdateVisual(int chipsAmount,int goldAmount)
    {
        _dismantelCurrencyUI.UpdateText(chipsAmount,goldAmount);
    }

    public void ConfirmDismantleCards()
    {
        _dismantleDataManager.ConfirmDismantleCards();
        UpdateVisual(0,0);
        _collectionHandler.UnLoadObjects();
        _dismantleDataManager.SetCardCollection();
        _collectionHandler.LoadObjects(VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dismantleDataManager.CardCollectionDatas.CollectionCardDatas),null);
    }

    private void OnDestroy()
    {
        _dismantleDataManager.OnCardAddToDismantel -= UpdateVisual;
    }

    public void ExitDismantleScreen()
    {
        _collectionHandler.UnLoadObjects();
        _dismantleDataManager.ResetDismantleCard();
        _dismantleDataManager.Dispose();//temp
        CloseScreen();
    }
}
