using Account.GeneralData;
using Battle.Combo;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.MatchMMaking
{
    public class ComboAssinger : MonoBehaviour
    {
        [SerializeField] private BattleComboUI[] _combos;

        public void AssingCombosUI(ComboCore[] comboDatas)
        {
            
            for (int i = 0; i < _combos.Length; i++)
            {
                if (comboDatas == null || i>= comboDatas.Length|| comboDatas[i] == null)
                    _combos[i].gameObject.SetActive(false);
                else
                    _combos[i].AssignVisual(comboDatas[i]);
            }
        }
    }
}

