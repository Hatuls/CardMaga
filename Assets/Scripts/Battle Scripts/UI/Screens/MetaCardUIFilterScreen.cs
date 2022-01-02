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
    [SerializeField] CollectionEnum _collectionType;
    [SerializeField] enum CollectionEnum { AccountCardsCollection, RunCardsCollection }
    protected override void CreatePool()
    {
        int deckCount = GetCollectionLength();


        while (deckCount > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponent<MetaCardUIHandler>();
            //  card.MetaCardUIOpenerAbst = _metaCardUI;

            _collection.Add(card);
        }
    }

    private int GetCollectionLength()
    {
        switch (_collectionType)
        {
            case CollectionEnum.AccountCardsCollection:
                return Account.AccountManager.Instance.AccountCards.CardList.Count;
            case CollectionEnum.RunCardsCollection:
                return Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterDeck.Length;
            default:
                Debug.LogError($"CollectionEnum Was not set {_collectionType}");
                return 0;
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
