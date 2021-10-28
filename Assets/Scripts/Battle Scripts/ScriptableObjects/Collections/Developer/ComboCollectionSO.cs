using UnityEngine;
using System.Collections.Generic;
namespace Collections
{
    [CreateAssetMenu(fileName = "ComboCollectionSO", menuName = "ScriptableObjects/Collections/ComboCollectionSO")]
    public class ComboCollectionSO : ScriptableObject, IScriptableObjectCollection
    {
        #region Fields
        [Tooltip("List of all relics in game")]
        [SerializeField] Combo.ComboSO[] _allComboSO;

        private Dictionary<ushort, Combo.ComboSO> _comboDict;
        #endregion

        public void Init(Combo.ComboSO[] combos)
        {
            _allComboSO = new Combo.ComboSO[combos.Length];
          System.Array.Copy(combos,_allComboSO,combos.Length);
        }


        #region properties
        public Combo.ComboSO[] GetComboSO => _allComboSO;


        public Combo.ComboSO GetCombo(ushort ID)
        {
            if (_comboDict == null)
             AssignDictionary();

            if (_comboDict.TryGetValue(ID, out var comboSO))
                return comboSO;
            else
                throw new System.Exception($"ComboCollectionSO : The ID: {ID} was not found in the dictionary!");
        }

        public async void AssignDictionary()
        {
            Debug.Log("<a>Assiging ComboDictionary</a>");
            const int module = 10;

            int length = _allComboSO.Length;
            _comboDict = new Dictionary<ushort, Combo.ComboSO>(length);

            for (int i = 0; i < length; i++)
            {
                var combo = _allComboSO[i];
                if (!_comboDict.ContainsKey(combo.ID))
                    _comboDict.Add(combo.ID, combo);
                else
                    throw new System.Exception($"ComboCollectionSO : Combo Dictionary found duplicate ID's:\nCombo:{combo.ComboName}\nID : {combo.ID}\n");

                if (i % module == 0 && i>0)
                    await System.Threading.Tasks.Task.Yield();
            }
            Debug.Log("<a>Finished Assiging ComboDictionary!!!</a>");
        }
        #endregion
    }
}
