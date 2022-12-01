using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.UI;
using UnityEngine;

public class MetaCharecterUICollection : MonoBehaviour , IInitializable<MetaCharacterData>, IEquatable<MetaCharacterData>
{
    public event Action<MetaDeckData> OnSetNewMainDeck; 

    [SerializeField] private MetaDeckUICollection[] _decksUI;

    private MetaCharacterData _characterData;

    private MetaDeckUICollection _mainDeckUI;
    
    public MetaCharacterData CharacterData => _characterData;
    
    public void Init(MetaCharacterData data)
    {
        _characterData = data;
        
        foreach (var metaDeckUICollection in _decksUI)
        {
            metaDeckUICollection.Input.OnClickValue += SetMainDeck;
        }
        
        for (int i = 0; i < data.Decks.Count; i++)
        {
            _decksUI[i].AssignVisual(data.Decks[i]);
            
            if (_decksUI[i].Equals(data.MainDeck))
            {
                SetMainDeck(_decksUI[i]);
            }
        }
    }

    private void SetMainDeck(MetaDeckUICollection metaDeckUI)
    {
        metaDeckUI.SetAsMainDeck();
        OnSetNewMainDeck?.Invoke(metaDeckUI.DeckData);
    }

    public void Dispose()
    {
        foreach (var metaDeckUICollection in _decksUI)
        {
            metaDeckUICollection.Input.OnClickValue -= SetMainDeck;
        }
    }

    public void CheckValidation()
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        Dispose();
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
