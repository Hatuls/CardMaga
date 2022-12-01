using System;
using CardMaga.Input;
using CardMaga.MetaData.AccoutData;
using CardMaga.UI;
using TMPro;
using UnityEngine;

public class MetaDeckUICollection : BaseUIElement , IVisualAssign<MetaDeckData>,IEquatable<MetaDeckData>
{
    [SerializeField] private TMP_Text _deckName;
    [SerializeField] private Button _input;

    private MetaDeckData _deckData;
    private int _deckId;

    public MetaDeckData DeckData => _deckData;
    
    public int DeckId => _deckId;

    public Button Input => _input;
    
    public void AssignVisual(MetaDeckData data)
    {
        _deckData = data;
        _deckId = data.DeckId;
        _deckName.text = data.DeckName;
    }

    public void UpdateDeckName()
    {
        _deckName.text = _deckData.DeckName;
    }

    public void SetAsMainDeck()
    {
        //do visual effect
    }

    public bool Equals(MetaDeckData other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (other.DeckId == _deckId) return true;
        return false;
    }
}
