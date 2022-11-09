using Battle.Combo;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.MatchMMaking
{
    public class ComboAssinger : MonoBehaviour
    {
        [SerializeField] private ComboUI[] _combos;

        public void AssingCombosUI(ComboData[] comboDatas)
        {
            for (int i = 0; i < comboDatas.Length; i++)
            {
                if (comboDatas[i] == null)
                    continue;
                    
                _combos[i].AssignVisual(comboDatas[i]);       
            }
        }

        public void ShowCombos()
        {
            for (int i = 0; i < _combos.Length; i++)
            {
                _combos[i].Show();
            }
        }
    }
}

