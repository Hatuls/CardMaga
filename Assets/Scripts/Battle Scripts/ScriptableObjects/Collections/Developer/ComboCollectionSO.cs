using UnityEngine;
using System.Collections.Generic;
using Cards;
using System.Linq;

namespace Collections
{
    [CreateAssetMenu(fileName = "ComboCollectionSO", menuName = "ScriptableObjects/Collections/ComboCollectionSO")]
    public class ComboCollectionSO : ScriptableObject, IScriptableObjectCollection
    {
        #region Fields
        [Tooltip("List of all relics in game")]
        [SerializeField] Combo.ComboSO[] _allComboSO;

        [SerializeField]
        RarityCombos[] _rarity;
        public RarityCombos[] CombosByRarity => _rarity;

        public RarityCombos GetComboByRarity(RarityEnum rarity)
        {
            for (int i = 0; i < _rarity.Length; i++)
            {
                if (_rarity[i].Rarity == rarity)
                    return _rarity[i];
            }
            throw new System.Exception("Rarity was Not Valid or Rarity Cards variable was not start up correctly");
        }

        [System.Serializable]
        public class RarityCombos
        {
            public RarityCombos(ushort[] _combos, RarityEnum rare)
            {
                _rarity = rare;
                _combo = _combos;
            }

            [SerializeField]
            RarityEnum _rarity;
            public RarityEnum Rarity => _rarity;

            [SerializeField] ushort[] _combo;
            public ushort[] ComboID => _combo;
        }






        private Dictionary<ushort, Combo.ComboSO> _comboDict;
        #endregion

        public void Init(Combo.ComboSO[] combos, Combo.ComboSO[] rewardedCombos )
        {
            _allComboSO = new Combo.ComboSO[combos.Length];
          System.Array.Copy(combos,_allComboSO,combos.Length);

            _rarity = new RarityCombos[5];
         
            _rarity[0] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Common), RarityEnum.Common), RarityEnum.Common);
            _rarity[1] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Uncommon), RarityEnum.Uncommon), RarityEnum.Uncommon);
            _rarity[2] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Rare), RarityEnum.Rare), RarityEnum.Rare);
            _rarity[3] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Epic), RarityEnum.Epic), RarityEnum.Epic);
            _rarity[4] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.LegendREI), RarityEnum.LegendREI), RarityEnum.LegendREI);
   

        }

        ushort[] GetRewardIDs(IEnumerable<Combo.ComboSO> combo , RarityEnum rarity)
        {
            List<ushort> _list = new List<ushort>(combo.Count());

          var ids = combo.Select(x => new { x.ID });

            foreach (var id in ids)
            {
                _list.Add(id.ID);
            }
            return _list.ToArray();
        }
        #region properties
        public Combo.ComboSO[] GetComboSO => _allComboSO;

        public Combo.ComboSO[] GetComboSOFromIDs(IEnumerable<ushort> ids)
        {
          return  ids.Select(x => GetCombo(x)).ToArray();
        }
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
