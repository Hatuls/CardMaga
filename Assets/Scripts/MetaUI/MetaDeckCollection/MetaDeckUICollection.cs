using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.UI;
using TMPro;
using UnityEngine;

public class MetaDeckUICollection : BaseUIElement , IVisualAssign<MetaDeckData>,IEquatable<MetaDeckData>
{
    [SerializeField] private RectTransform _glow;
    [SerializeField] private RectTransform _deckActive;
    [SerializeField] private RectTransform _deckUnactive;
    [SerializeField] private TMP_Text _deckName;
    [SerializeField] private DeckInput _input;

    private MetaDeckData _deckData;
    private int _deckId;

    public MetaDeckData DeckData => _deckData;
    
    public int DeckId => _deckId;

    public DeckInput Input => _input;

    private void OnEnable()
    {
        UpdateDeckName();
    }

    public void AssignVisual(MetaDeckData data)
    {
        if (ReferenceEquals(data,null))
        {
            _deckData = null;
            _deckId = -1;
            _deckName.text = "New Deck";
            
            return;
        }
        
        _deckData = data;
        _deckId = data.DeckId;
        _deckName.text = data.DeckName;
        Show();
    }

    private void UpdateDeckName()
    {
        if (_deckData == null)
            return;
        
        _deckName.text = _deckData.DeckName;
    }

    public void SetAsMainDeck()
    {
        _glow.gameObject.SetActive(true);
    }
    
    public void UnSetAsMainDeck()
    {
        _glow.gameObject.SetActive(false);
        //do visual effect
    }

    public override void Show()
    {
        base.Show();
        _deckActive.gameObject.SetActive(true);
        _deckUnactive.gameObject.SetActive(false);
    }

    public override void Hide()
    {
        base.Hide();
        _deckActive.gameObject.SetActive(false);
        _deckUnactive.gameObject.SetActive(true);
    }

    public bool Equals(MetaDeckData other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (other.DeckId == _deckId) return true;
        return false;
    }
}
