using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class MetaCharacterScreenUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    [SerializeField] private UnityEvent OnOpenEditingScreen;
    private MetaDeckBuildingUIManager _metaDeckBuildingUIManager;
    private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();
    
    private MetaCharecterUICollection _mainCharecterUI;
    
    public int Priority => 1;
    
    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        MetaCharactersHandler metaCharactersHandler = data.MetaDataManager.MetaAccountData.CharacterDatas;
        _metaDeckBuildingUIManager = data.MetaDeckBuildingUIManager;
        foreach (var charecterUICollection in _charectersUI)
        {
            _sequenceHandler.Register(charecterUICollection);
        }
        
        _sequenceHandler.StartAll(data);

        foreach (var charecterUICollection in _charectersUI)
        {
            charecterUICollection.OnNewDeckAdded += OpenScreen;
        }

        _mainCharecterUI = _charectersUI[0];// plaster
    }

    public void DiscardMainDeck()
    {
        _mainCharecterUI.DiscardLastDeck(_mainCharecterUI.MainDeckUI.DeckId);
    }

    private void OnDestroy()
    {
        foreach (var charecterUICollection in _charectersUI)
        {
            charecterUICollection.OnNewDeckAdded -= OpenScreen;
        }
    }

    public void OpenDeckEditScreen()
    {
        _metaDeckBuildingUIManager.OpenScreen();
    }
    public override void OpenScreen()
    {
        base.OpenScreen();
        OnOpenEditingScreen?.Invoke();
    }
}
