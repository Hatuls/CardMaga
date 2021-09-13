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


        public Combo.ComboSO GetCombo(int ID)
        {
            for (int i = 0; i < _allComboSO.Length; i++)
            {
                if (_allComboSO[i].ID == ID)
                    return _allComboSO[i];
            }
            return null;
        }
        #endregion
    }
}
