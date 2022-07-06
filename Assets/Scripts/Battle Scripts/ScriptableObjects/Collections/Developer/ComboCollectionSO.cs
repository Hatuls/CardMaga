using UnityEngine;
using System.Collections.Generic;
using Cards;
using System.Linq;
using Battle.Combo;
namespace Collections
{
    [CreateAssetMenu(fileName = "ComboCollectionSO", menuName = "ScriptableObjects/Collections/ComboCollectionSO")]
    public class ComboCollectionSO : ScriptableObject
    {
        #region Fields
        [Tooltip("List of all relics in game")]
        [SerializeField] ComboSO[] _allComboSO;

        [SerializeField]
        RarityCombos[] _rarity;
        public RarityCombos[] CombosByRarity => _rarity;



        [System.Serializable]
        public class RarityCombos
        {
            public RarityCombos(int[] _combos, RarityEnum rare)
            {
                _rarity = rare;
                _combo = _combos;
            }

            [SerializeField]
            RarityEnum _rarity;
            public RarityEnum Rarity => _rarity;

            [SerializeField] int[] _combo;
            public int[] ComboID => _combo;
        }



        #region properties
        public ComboSO[] AllCombos => _allComboSO;



        #endregion



        #endregion

        public void Init(ComboSO[] combos, ComboSO[] rewardedCombos )
        {
            _allComboSO = new ComboSO[combos.Length];
          System.Array.Copy(combos,_allComboSO,combos.Length);

            _rarity = new RarityCombos[5];
         
            _rarity[0] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Common)), RarityEnum.Common);
            _rarity[1] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Uncommon)), RarityEnum.Uncommon);
            _rarity[2] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Rare)), RarityEnum.Rare);
            _rarity[3] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.Epic)), RarityEnum.Epic);
            _rarity[4] = new RarityCombos(GetRewardIDs(rewardedCombos.Where((x) => x.GetRarityEnum == RarityEnum.LegendREI)), RarityEnum.LegendREI);
   

        }
        public RarityCombos GetComboByRarity(RarityEnum rarity)
        {
            for (int i = 0; i < _rarity.Length; i++)
            {
                if (_rarity[i].Rarity == rarity)
                    return _rarity[i];
            }
            throw new System.Exception("Rarity was Not Valid or Rarity Cards variable was not start up correctly");
        }
        int[] GetRewardIDs(IEnumerable<ComboSO> combos)
        {
            List<int> _list = new List<int>(combos.Count());

            foreach (var instance in combos)
                _list.Add(instance.ID);
            
            return _list.ToArray();
        }
     
    }
}
