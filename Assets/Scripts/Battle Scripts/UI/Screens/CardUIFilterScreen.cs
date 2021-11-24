using Battles;
using Battles.UI;
using Cards;
using Map.UI;
using System.Collections.Generic;
using System.Linq;

public class CardUIFilterScreen : UIFilterScreen<CardUI, Card>
{

    protected override void CreatePool()
    {
        var deck = BattleData.Player.CharacterData.CharacterDeck;
        while (deck.Length > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, this.transform).GetComponent<CardUI>();
            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Card> sortedDeck, int i)
    {
        var artSO = Factory.GameFactory.Instance.ArtBlackBoard;
        _collection[i].GFX.SetCardReference(sortedDeck.ElementAt(i), artSO);

    }
}

