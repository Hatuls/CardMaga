using CardMaga.UI.Card;
using UnityEngine;

public class SelectCardUI : MonoBehaviour
{
    [SerializeField] private FollowCardUI _followCard;
    [SerializeField] private ZoomCardUI _zoomCard;
    
    private CardUI _selectCardUI;

    public CardUI SelectedCardUI
    {
        get => _selectCardUI;
    }

    public bool TrySetSelectedCardUI(CardUI cardUI)
    {
        if (_selectCardUI != null)
        {
            Debug.LogWarning("Selected Card already have a value");
            return false;
        }

        _selectCardUI = cardUI;
        return true;
    }
}
