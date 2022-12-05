using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class MetaDeckUICollectionManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    
    private MetaCharecterUICollection _mainCharecterUI;
    
    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        MetaCharactersHandler metaCharactersHandler = data.MetaDataManager.MetaAccountData.CharacterDatas;
        
        for (int i = 0; i < metaCharactersHandler.CharacterDatas.Length; i++)
        {
            _charectersUI[i].Init(metaCharactersHandler.CharacterDatas[i]);
        }
    }

    public int Priority => 1;

    private void SetMainCharacterUI(MetaCharacterData metaCharacterData)
    {
        
    }
}
