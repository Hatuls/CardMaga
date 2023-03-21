using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using UnityEngine;

public class MetaCharacterScreenUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
{
    [SerializeField] private MetaCharecterUICollection[] _charectersUI;
    private MetaDeckEditingUIManager metaDeckEditingUIManager;
        
    private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();
    
    private MetaCharecterUICollection _mainCharecterUI;
    
    public int Priority => 1;
    
    public void ExecuteTask(ITokenReceiver tokenMachine, MetaUIManager data)
    {
        metaDeckEditingUIManager = data.MetaDeckEditingUIManager;
        
        foreach (var charecterUICollection in _charectersUI)
        {
            _sequenceHandler.Register(charecterUICollection);
        }
        
        _sequenceHandler.StartAll(data);

        foreach (var charecterUICollection in _charectersUI)
        {
            charecterUICollection.OnSelectedDeckPressed += OpenDeckEditScreen;
            charecterUICollection.OnNewDeckAdd += OpenDeckEditScreen;
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
            charecterUICollection.OnSelectedDeckPressed -= OpenDeckEditScreen;
            charecterUICollection.OnNewDeckAdd -= OpenDeckEditScreen;
        }
    }

    public void OpenDeckEditScreen()
    {
        metaDeckEditingUIManager.OpenScreen();
    }
}