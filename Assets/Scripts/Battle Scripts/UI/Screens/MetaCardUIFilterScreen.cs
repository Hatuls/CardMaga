using Cards;
using Map.UI;
using System.Collections.Generic;
using System.Linq;
using UI.Meta.Laboratory;
using UnityEngine;

public class MetaCardUIFilterScreen : UIFilterScreen<MetaCardUIHandler, Card>
{
    //    [SerializeField]
    //    CollectionLabOpen _metaCardUI;
    [SerializeField]
    Transform _container;
    [SerializeField] float _metaCardSize = 1f;
    [SerializeField] bool toUseAccountInfo;

    protected override void CreatePool()
    {
        var deckCount = toUseAccountInfo ?
            Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck.Length:
            Account.AccountManager.Instance.AccountCards.CardList.Count ;

        while (deckCount > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponent<MetaCardUIHandler>();
            //  card.MetaCardUIOpenerAbst = _metaCardUI;

            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Card> sortedDeck, int i)
    {
        _collection[i].CardUI.GFX.SetCardReference(
            sortedDeck.ElementAt(i)
            );
        _collection[i].transform.localScale = Vector3.one * _metaCardSize;

    }
}
public class DojoUpgradeScrollHandler : MonoBehaviour
{
    [SerializeField] MetaCardUIFilterScreen _cardFilter;
    [SerializeField] MetaComboUIFilterScreen _comboFilter;

}
