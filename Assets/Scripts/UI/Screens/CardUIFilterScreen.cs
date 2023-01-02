using Battle;
using CardMaga.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardMaga.UI.Card;
using CardMaga.Card;

public class CardUIFilterScreen : UIFilterScreen<BattleCardUI, BattleCardData>
{
    [SerializeReference]
    
    [SerializeField]
    float _cardsSize =1f ;
    // Need To be Re-Done
    protected override void CreatePool()
    {
        //var deck = Account.AccountManager.Instance.BattleData.LeftPlayer.CharacterData.CharacterDeck;
        //while (deck.Length > _collection.Count)
        //{
        //    var battleCard = Instantiate(_cardUIPrefab, this.transform).GetComponent<CardUI>();
        //    _collection.Add(battleCard);
        //}
    }

    protected override void OnActivate(IEnumerable<BattleCardData> sortedDeck, int i)
    {
        _collection[i].AssignDataAndVisual(sortedDeck.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _cardsSize;
    }
}

