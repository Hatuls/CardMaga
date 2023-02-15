using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using UnityEngine;

public class MetaCharecterUICollection : BaseUIElement, ISequenceOperation<MetaUIManager>, IEquatable<MetaCharacterData>
{
    public event Action OnSelectedDeckPressed;
    public event Action OnNewDeckAdd;
    
    [SerializeField] private MetaDeckUICollection[] _decksUI;

    private int _mainDeckId;

    private MetaCharacterData _characterData;
    
    public MetaDeckUICollection MainDeckUI => _decksUI[_mainDeckId];

    public int Priority => 1;

    public void ExecuteTask(ITokenReceiver tokenMachine, MetaUIManager data)
    {
        _characterData = data.MetaDataManager.MetaAccountData.CharacterDatas.MainCharacterData;
        
        for (int i = 0; i < _decksUI.Length; i++)
        {
            _decksUI[i].OnSetMainDeck += SetMainDeck;
            _decksUI[i].OnClickSelectedDeck += SelectedDeckPressed;
            _decksUI[i].AssignVisual(i < _characterData.Decks.Count ? _characterData.Decks[i] : null);
        }
        
        SetMainDeck(_characterData.MainDeckIndex);
    }

    private void OnEnable()
    {
        var deckDatas = _characterData.Decks;
        
        for (int i = 0; i < _decksUI.Length; i++)
        {
            _decksUI[i].AssignVisual(deckDatas[i]);    
        }
    }

    private void SetMainDeck(int deckId)
    {
        if (MainDeckUI != null)
            MainDeckUI.UnSetAsMainDeck();

        if (deckId == -1)
        {
           // MetaDeckData metaDeckData = _characterData.AddDeck();
           MetaDeckData metaDeckData = _characterData.GetNewDeckCopy();

            if (ReferenceEquals(metaDeckData, null))
                return;

            _mainDeckId = metaDeckData.DeckId;

            MainDeckUI.AssignVisual(metaDeckData);
            MainDeckUI.Show();
            
            _characterData.SetMainDeck(_mainDeckId);
            OnNewDeckAdd?.Invoke();
        }
        else
        {
            _mainDeckId = deckId;
            MainDeckUI.SetMainDeck();
            _characterData.SetMainDeck(_mainDeckId);
        }
    }

    public void DiscardLastDeck(int deckId)
    {
        SetMainDeck(0);

        _decksUI[deckId].DiscardDeck();

        _characterData.DiscardDeck(deckId);
        _characterData.SetMainDeck(_mainDeckId);
    }

    private void SelectedDeckPressed()
    {
        OnSelectedDeckPressed?.Invoke();
    }

    public void OnDestroy()
    {
        foreach (var metaDeckUICollection in _decksUI)
        {
            metaDeckUICollection.OnClickSelectedDeck -= SelectedDeckPressed;
            metaDeckUICollection.OnSetMainDeck -= SetMainDeck;
        }
    }

    private bool TryFindMetaDeckUI(MetaDeckData metaDeckData, out MetaDeckUICollection metaDeckUICollection)
    {
        foreach (var deckUI in _decksUI)
        {
            if (deckUI.Equals(metaDeckData))
            {
                metaDeckUICollection = deckUI;
                return true;
            }
        }

        metaDeckUICollection = null;
        return false;
    }

    public bool Equals(MetaCharacterData other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (other.Id == _characterData.Id) return true;
        return false;
    }
}
