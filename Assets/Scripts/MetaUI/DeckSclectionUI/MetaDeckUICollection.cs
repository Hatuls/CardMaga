using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.UI;
using TMPro;
using UnityEngine;

public class MetaDeckUICollection : BaseUIElement , IVisualAssign<MetaDeckData>,IEquatable<MetaDeckData>
{
    public event Action<int> OnSetMainDeck;
    public event Action OnClickSelectedDeck;
    
    [SerializeField] private RectTransform _glow;
    [SerializeField] private RectTransform _deckActive;
    [SerializeField] private RectTransform _deckUnactive;
    [SerializeField] private TMP_Text _deckName;
    [SerializeField] private DeckInput _input;

    private MetaDeckData _deckData;

    public MetaDeckData DeckData => _deckData;

    public bool IsSelected => _glow.gameObject.activeSelf;
    
    public int DeckId => _deckData?.DeckId ?? -1;
    
    public bool IsNewDeck => _deckData.IsNewDeck;

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
            _deckName.text = "New Deck";
            Hide();
            return;
        }
        
        _deckData = data;
        _deckName.text = data.DeckName;
        Show();
    }

    private void UpdateDeckName()
    {
        if (_deckData == null)
            return;
        
        _deckName.text = _deckData.DeckName;
    }

    public void DiscardDeck()
    {
        _deckData = null;
        _deckName.text = "New Deck";
        Hide();
    }

    public void OnPress()
    {
        if (IsSelected)
        {
            OnClickSelectedDeck?.Invoke();
            return;
        }
        
        OnSetMainDeck?.Invoke(DeckId);
        SetMainDeck();
    }

    public void SetMainDeck()
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
        _deckActive.gameObject.SetActive(false);
        _deckUnactive.gameObject.SetActive(true);
        _glow.gameObject.SetActive(false);
    }

    public bool Equals(MetaDeckData other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (other.DeckId == DeckId) return true;
        return false;
    }
}
