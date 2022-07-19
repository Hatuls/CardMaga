using Battle.UI;
using TMPro;
using UnityEngine;

public class DismentalScreen : MonoBehaviour
{
    [SerializeField]
    CardUI _cardUI;

    [SerializeField]
    DismentalCostsSO _dismental;

    [SerializeField]
    UnityEngine.Events.UnityEvent OnDismentalSuccess;
    [SerializeField]
    TextMeshProUGUI costText;
    ushort _amount;
    public void Open(CardUI card)
    {
        var refcard = card.CardData;
        _cardUI.AssignCard(refcard);
        _amount = _dismental.GetCardDismentalCost(refcard);
        costText.text = _amount.ToString();
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void DismentalCard()
    {
      if(DismentalHandler.DismentalCard(_cardUI.CardData))
        {

            OnDismentalSuccess?.Invoke();
        }
    }
}
