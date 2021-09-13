using UnityEngine;
namespace Collections
{
    [CreateAssetMenu(fileName = "ComboRecipeCollectionSO", menuName = "ScriptableObjects/Collections/ComboRecipeCollectionSO")]
    public class ComboRecipeCollectionSO : ScriptableObject
    {
        #region Fields
        [Tooltip("List of all relics in game")]
        [SerializeField] Combo.ComboSO[] _allComboSO;
        #endregion

        public void Init(Combo.ComboSO[] combos)
        {
            _allComboSO = combos;
        }


        #region properties
        public Combo.ComboSO[] GetComboSO => _allComboSO;
        #endregion
    }
}
