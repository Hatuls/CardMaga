using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
[CreateAssetMenu(fileName =("CardsCollections"), menuName =("ScriptableObjects/Collections/CardsCollections"))]
public class CardsCollectionSO : SerializedScriptableObject
{
    [SerializeField]
    Cards.CardSO[] _cardCollection;

    public void Init(Cards.CardSO[] collection)
        => _cardCollection = collection;


    public Cards.CardSO[] GetAllCards
    {
        get
        {
            if (_cardCollection != null)
                return _cardCollection;
            else
            {
                Debug.LogError("Error Getting All Cards");
                return null;
            }
        }
    }


    public Cards.CardSO GetCard(int ID)
    {
        for (int i = 0; i < _cardCollection.Length; i++)
        {
            if (_cardCollection[i].ID == ID)
                return _cardCollection[i];
        }
        return null;
    }
}
