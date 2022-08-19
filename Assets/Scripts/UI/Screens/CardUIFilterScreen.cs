using Battle;
using CardMaga.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardMaga.UI.Card;
using CardMaga.Card;

public class CardUIFilterScreen : UIFilterScreen<CardUI, CardData>
{
    [SerializeReference]
    
    [SerializeField]
    float _cardsSize =1f ;
    // Need To be Re-Done
    protected override void CreatePool()
    {
        //var deck = Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck;
        //while (deck.Length > _collection.Count)
        //{
        //    var card = Instantiate(_cardUIPrefab, this.transform).GetComponent<CardUI>();
        //    _collection.Add(card);
        //}
    }

    protected override void OnActivate(IEnumerable<CardData> sortedDeck, int i)
    {
        _collection[i].AssignCard(sortedDeck.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _cardsSize;
    }
}

