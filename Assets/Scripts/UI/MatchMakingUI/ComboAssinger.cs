using Battle.Combo;
using UnityEngine;

namespace CardMaga.UI.MatchMMaking
{
    public class ComboAssinger : MonoBehaviour
    {
        [SerializeField] private ComboUI[] _combos;

        public void AssingCombosUI(ComboData[] comboDatas)
        {
            for (int i = 0; i < _combos.Length; i++)
            {
                _combos[i].AssingVisual(comboDatas[i]);       
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

