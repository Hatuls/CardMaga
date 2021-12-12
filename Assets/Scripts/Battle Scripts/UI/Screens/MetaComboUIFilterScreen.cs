using Map.UI;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class MetaComboUIFilterScreen : UIFilterScreen<ComboRecipeUI, Combo.Combo>
{
    [Tooltip("True will use the accounts information\nFalse will use the info from this Run")]
    [SerializeField] bool toUseAccountData;
    [SerializeField]
    Transform _container;
    [SerializeField] float _comboSize = 1f;
    protected override void CreatePool()
    {
        var deckCount = toUseAccountData ?
                 Account.AccountManager.Instance.AccountCombos.ComboList.Count :
                 Battles.BattleData.Player.CharacterData.ComboRecipe.Length;

        while (deckCount > _collection.Count)
        {
            var card = Instantiate(_cardUIPrefab, _container).GetComponent<ComboRecipeUI>();
            _collection.Add(card);
        }
    }

    protected override void OnActivate(IEnumerable<Combo.Combo> sortedCombo, int i)
    {
        _collection[i].InitRecipe(sortedCombo.ElementAt(i));
        _collection[i].transform.localScale = Vector3.one * _comboSize;
    }
}
