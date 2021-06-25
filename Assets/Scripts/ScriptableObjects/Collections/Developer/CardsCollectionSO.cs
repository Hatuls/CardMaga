
using UnityEngine;
[CreateAssetMenu(fileName =("CardsCollections"), menuName =("ScriptableObjects/Collections/CardsCollections"))]
public class CardsCollectionSO : ScriptableObject
{
    [SerializeField] Cards.CardSO[] cardCollection;
    public Cards.CardSO[] GetAllCards
    {
        get
        {
            if (cardCollection != null)
                return cardCollection;
            else
            {
                Debug.LogError("Error Getting All Cards");
                return null;
            }
        }
    }
}
