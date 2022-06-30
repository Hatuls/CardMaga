using CardMaga.UI;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Battle.Combo;
public class MetaComboUIFilterScreen : UIFilterScreen<ComboRecipeUI, Combo>
{
    [Tooltip("True will use the accounts information\nFalse will use the info from this Run")]
    [SerializeField] bool toUseAccountData;
    [SerializeField]
    Transform _container;
    [SerializeField] float _comboSize = 1f;
    // Need To be Re-Done
    protected override void CreatePool()
    {
        //var deckCount = toUseAccountData ?
        //         Account.AccountManager.Instance.AccountCombos.ComboList.Count :
        //         Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe.Length;

        //while (deckCount > _collection.Count)
        //{
        //    var card = Instantiate(_cardUIPrefab, _container).GetComponentInChildren<ComboRecipeUI>();
        //    _collection.Add(card);
        //}
    }

    protected override void OnActivate(IEnumerable<Combo> sortedCombo, int i)
    {
        _collection[i].InitRecipe(sortedCombo.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _comboSize;
    }
    // Need To be Re-Done
    public void ShowAll()
    {
        //CreatePool();
        //int length = _collection.Count;
        //var combos = toUseAccountData ? Factory.GameFactory.Instance.ComboFactoryHandler.CreateCombo(Account.AccountManager.Instance.AccountCombos.ComboList.ToArray()) :
        //         Account.AccountManager.Instance.BattleData.Player.CharacterData.ComboRecipe;
        //for (int i = 0; i < length; i++)
        //{
        //    if (i < combos.Length)
        //    {
        //        if (_collection[i].gameObject.activeSelf == false)
        //            _collection[i].gameObject.SetActive(true);

        //        _collection[i].InitRecipe(combos[i]);
        //    }
        //    else
        //    {
        //        if (_collection[i].gameObject.activeSelf == true)
        //            _collection[i].gameObject.SetActive(false);
        //    }
        //}
    }
}
