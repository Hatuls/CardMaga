using CardMaga.Card;
using CardMaga.Rewards;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace CardMaga.MetaUI.Shop
{
    public class PackInfoPopup : MonoBehaviour
    {
        [SerializeField]
        private PackInfo[] _packInfos;

        public void InitText(RarityChanceCardContainer[] rarityChanceCardContainers)
        {
            const string p = "%";
            for (int i = 0; i < rarityChanceCardContainers.Length; i++)
            {

                var packInfo = First(rarityChanceCardContainers[i].PackCardsRewards.Rarity);
                float amount = rarityChanceCardContainers[i].Chance;
                packInfo.Parent.SetActive(amount > 0);

                if (packInfo.Parent.activeInHierarchy)
                    packInfo.Text.text = string.Concat(amount.ToString(),p) ;
            }

            PackInfo First(RarityEnum rarity)
            {
                for (int i = 0; i < _packInfos.Length; i++)
                {
                    if (_packInfos[i].Rarity == rarity)
                        return _packInfos[i];
                }
                throw new Exception("Rarirty was not found in pack info\nRarity Type: " + rarity.ToString());
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Assign Pack Info")]
        private void Init()
        {
            List<PackInfo> p = new List<PackInfo>();
            foreach (var rarity in Enum.GetValues(typeof(RarityEnum)) as RarityEnum[])
            {
                if (rarity == RarityEnum.None)
                    continue;
                var @object = GetObject(transform, rarity== RarityEnum.LegendREI? "Legendary" : rarity.ToString());
                var x = new PackInfo();
                x.Rarity = rarity;
                x.Parent = @object.gameObject;
                x.Text = x.Parent.GetComponentInChildren<TextMeshProUGUI>();
                p.Add(x);
            }

            _packInfos = p.ToArray();
            Transform GetObject(Transform parent, string name)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    var go = parent.GetChild(i);

                    if (go.name.Contains(name))
                        return go;

                    go = GetObject(go, name);

                        if (go != null)
                        return go;
                }
                return null;
            }
        }

#endif


        [Serializable]
        protected class PackInfo
        {
            public RarityEnum Rarity;
            public GameObject Parent;
            public TextMeshProUGUI Text;
        }
    }
}