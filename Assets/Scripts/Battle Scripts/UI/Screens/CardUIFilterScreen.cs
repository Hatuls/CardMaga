using Battles;
using Battles.UI;
using Cards;
using Map.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardUIFilterScreen : UIFilterScreen<CardUI, Card>
{
    [SerializeField]
    float _cardsSize =1f ;
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
        _collection[i].GFX.SetCardReference(sortedDeck.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _cardsSize;
    }
}

