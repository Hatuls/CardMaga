using CardMaga.UI.Card;
using UnityEngine;
public class CardPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _cardToLoad;
    private CardUI _cardUI;
    void Start()
    {

    }

    public CardUI DrawCard()
    {
        return _cardUI;
    }


}
