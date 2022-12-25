using CardMaga.MetaData.Dismantle;
using CardMaga.MetaUI;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class DismantelUIManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
{
    [SerializeField] private DismantleDataManager _dismantleDataManager;
    [SerializeField] private MetaCollectionHandler _collectionHandler;
    
    public int Priority => 0;

    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        _collectionHandler.Init();
        _dismantleDataManager = data.MetaDataManager.DismantleDataManager;
        
        _collectionHandler.LoadObjects(VisualRequesterManager.Instance.GetMetaCollectionCardUI(_dismantleDataManager.CardCollectionDatas),null);
    }

    public void ExitDismantelScreen()
    {
        _dismantleDataManager.ResetDismantelCard();
        gameObject.SetActive(false);
    }
}
