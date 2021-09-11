using UnityEngine;
namespace Collections.RelicsSO
{
    [CreateAssetMenu(fileName = "ComboCollectionSO", menuName = "ScriptableObjects/Collections/ComboCollectionSO")]
    public class ComboCollectionSO : ScriptableObject
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
