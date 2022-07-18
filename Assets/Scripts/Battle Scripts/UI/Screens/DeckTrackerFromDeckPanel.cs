using System.Collections.Generic;
using UI.Meta.Laboratory;
using UnityEngine;

public class DeckTrackerFromDeckPanel : MonoBehaviour
{
    [SerializeField] CharacterEnum _currentCharacter = CharacterEnum.Chiara;
    [SerializeField] int _currentIndex = 0;
    [SerializeField] MetaCardUIFilterScreen _deckCollection;
    [SerializeField] GetDeckLengthHelper _getDeckLength;

    private void OnEnable()
    {


        _currentIndex = 0;
    }
    public void SetDeckIndex(int currentIndex)
    {
        _currentIndex = currentIndex;

    }

    private void OnDisable()
    {
        if (this.gameObject.activeInHierarchy)
        {
            if (Application.isPlaying)
                RegisterDeck();
        }
    }

    public void RegisterDeck()
    {
        int length = _getDeckLength.GetDeckLength((IReadOnlyList<MetaCardUIHandler>)_deckCollection.Collection) ;
        var characterSO = Account.AccountManager.Instance.AccountCharacters.GetCharacterData(_currentCharacter);
        var deck = characterSO.GetDeckAt(_currentIndex);
        Account.GeneralData.CardCoreInfo[] cards = new Account.GeneralData.CardCoreInfo[deck.Cards.Length];
        for (int i = 0; i < length; i++)
        {
            var currentCard = _deckCollection.Collection[i].CardUI.CardData;
            if (currentCard == null)
                throw new System.Exception($"DeckTrackerFromDeckPanel: Card From Deck Collection is null!");
            cards[i] = new Account.GeneralData.CardCoreInfo(currentCard.CardSO.ID, currentCard.CardInstanceID, currentCard.CardLevel);
        }
        deck.Cards = cards;
    }

 
}
public interface IGetDeckCollectionLength<T>
{
    int GetDeckLength(IReadOnlyList<T> get);

}
public abstract class GetDeckLengthHelper:MonoBehaviour, IGetDeckCollectionLength<MetaCardUIHandler>
{
    public abstract int GetDeckLength(IReadOnlyList<MetaCardUIHandler> get);
}



public class GetCollectionLength : GetDeckLengthHelper
{
    public override int GetDeckLength(IReadOnlyList<MetaCardUIHandler> get)
    {
        return get.Count;
    }
}
