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

    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        _collectionHandler?.Init();
        _dismantleDataManager = data.MetaDataManager.DismantleDataManager;

        _dismantleDataManager.OnCardAddToDismantel += UpdateVisual;
    }

    private void OnEnable()
    {
        _collectionHandler.LoadObjects(VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dismantleDataManager.CardCollectionDatas.CollectionCardDatas),null);
    }

    private void UpdateVisual(int chipsAmount,int goldAmount)
    {
        _dismantelCurrencyUI.UpdateText(chipsAmount);
    }

    public void ExitDismantelScreen()
    {
        _dismantleDataManager.Dispose();//temp
        _dismantleDataManager.OnCardAddToDismantel -= UpdateVisual;
        _dismantleDataManager.ResetDismantelCard();
        CloseScreen();
    }
}
