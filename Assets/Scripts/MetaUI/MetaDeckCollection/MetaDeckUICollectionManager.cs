using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class MetaDeckUICollectionManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    
    private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();
    
    private MetaCharecterUICollection _mainCharecterUI;
    
    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        MetaCharactersHandler metaCharactersHandler = data.MetaDataManager.MetaAccountData.CharacterDatas;

        foreach (var charecterUICollection in _charectersUI)
        {
            _sequenceHandler.Register(charecterUICollection);
        }
        
        _sequenceHandler.StartAll(data);
    }

    public int Priority => 1;

    private void SetMainCharacterUI(MetaCharacterData metaCharacterData)
    {
        
    }
}
