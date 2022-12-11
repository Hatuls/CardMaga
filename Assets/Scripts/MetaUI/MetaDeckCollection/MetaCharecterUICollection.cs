﻿using System;
using CardMaga.MetaData.AccoutData;using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

public class MetaCharecterUICollection : MonoBehaviour, ISequenceOperation<MetaUIManager>, IEquatable<MetaCharacterData>
{
    public event Action<int> OnSetMainDeck; 
    public event Func<MetaDeckData> OnSetNewMainDeck; 

    [SerializeField] private MetaDeckUICollection[] _decksUI;

    private MetaCharacterData _characterData;

    private MetaDeckUICollection _mainDeckUI = null;
    
    public MetaCharacterData CharacterData => _characterData;

    public int Priority => 1;

    public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
    {
        _characterData = data.MetaDataManager.MetaAccountData.CharacterDatas.CharacterData;

        OnSetMainDeck += _characterData.SetMainDeck;
        OnSetNewMainDeck += _characterData.AddDeck;
        
        foreach (var metaDeckUICollection in _decksUI)
        {
            metaDeckUICollection.Input.OnClickValue += SetMainDeck;
        }
        
        for (int i = 0; i < _decksUI.Length; i++)
        {
            _decksUI[i].AssignVisual(i < _characterData.Decks.Count ? _characterData.Decks[i] : null);

            if (_decksUI[i].Equals(_characterData.MainDeck))
            {
                SetMainDeck(_decksUI[i]);
            }
        }
    }

    private void SetMainDeck(MetaDeckUICollection metaDeckUI)
    {
        _mainDeckUI?.UnSetAsMainDeck();
        
        _mainDeckUI = metaDeckUI;

        if (ReferenceEquals(metaDeckUI.DeckData, null))
        {
            MetaDeckData metaDeckData = OnSetNewMainDeck?.Invoke();
            
            if (ReferenceEquals(metaDeckData,null))
                return;
            
            _characterData.SetMainDeck(metaDeckData.DeckId);
            _mainDeckUI.AssignVisual(metaDeckData);
            _mainDeckUI.Show();
        }
        else
            OnSetMainDeck?.Invoke(metaDeckUI.DeckId);
        
        _mainDeckUI.SetAsMainDeck();
    }

    public void OnDestroy()
    {
        foreach (var metaDeckUICollection in _decksUI)
        {
            metaDeckUICollection.Input.OnClickValue -= SetMainDeck;
        }
        
        OnSetMainDeck -= _characterData.SetMainDeck;
        OnSetNewMainDeck -= _characterData.AddDeck;
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
