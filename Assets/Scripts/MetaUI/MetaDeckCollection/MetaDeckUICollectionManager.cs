using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class MetaDeckUICollectionManager : MonoBehaviour , ISequenceOperation<MetaUIManager>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    [SerializeField] private UnityEvent OnOpenEditingScreen;
    
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

        foreach (var charecterUICollection in _charectersUI)
        {
            charecterUICollection.OnNewDeckAdded += OpenDeckEditingScreen;
        }
    }

    private void OnDestroy()
    {
        foreach (var charecterUICollection in _charectersUI)
        {
            charecterUICollection.OnNewDeckAdded -= OpenDeckEditingScreen;
        }
    }

    public int Priority => 1;

    private void SetMainCharacterUI(MetaCharacterData metaCharacterData)
    {
        
    }

    public void OpenDeckEditingScreen()
    {
        OnOpenEditingScreen?.Invoke(); 
    }
}
