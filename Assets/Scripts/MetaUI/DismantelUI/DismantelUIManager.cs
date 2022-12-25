using CardMaga.MetaData.Dismantle;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class DismantelUIManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
{
    [SerializeField] private DismantleDataManager _dismantleDataManager;
    
    public int Priority => 0;

    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        _dismantleDataManager = data.MetaDataManager.DismantleDataManager;
    }

    public void ExitDismantelScreen()
    {
        _dismantleDataManager.ResetDismantelCard();
        gameObject.SetActive(false);
    }
}
