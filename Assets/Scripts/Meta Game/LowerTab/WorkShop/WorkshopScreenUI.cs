using Rewards;
using Rewards.Packs;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Meta.Workshop
{
    public class WorkshopScreenUI : TabAbst
    {
        [SerializeField]
        List<PackUI> packUIs;

        [SerializeField]
        GameObject _PackUIPrefab;

        [SerializeField]
        PackRewardsCollectionSO _packRewardsCollectionSO;

        [SerializeField]
        RecievePackPanel _recievePack;
        [SerializeField]
        Transform _containerPackParent;



        private PackRewardSO _lastPack;
        #region Initialize
        private void InitRewardScreen()
        {
            List<PackRewardSO> packs = new List<PackRewardSO>();
            foreach (var item in System.Enum.GetValues(typeof(Cards.RarityEnum)))
            {
                if ((Cards.RarityEnum)item == Cards.RarityEnum.None)
                    continue;
                var packReward = _packRewardsCollectionSO.PackRewardSO((Cards.RarityEnum)item);
                if (!packs.Contains(packReward))
                    packs.Add(packReward);
            }
            int length = packs.Count;


            CreatePacks(length);


            for (int i = 0; i < packUIs.Count; i++)
            {
                if (i < length)
                {
                    packUIs[i].Init(packs[i]);
                    if (!packUIs[i].gameObject.activeSelf)
                        packUIs[i].gameObject.SetActive(true);

                }
                else
                    packUIs[i].gameObject.SetActive(false);
            }

        }
        private void CreatePacks(int listLength)
        {
            while ((packUIs.Count < listLength))
            {
                var pack = Instantiate(_PackUIPrefab, _containerPackParent).GetComponent<PackUI>();
                packUIs.Add(pack);
            }
        }
        #endregion
        #region Pack Rewards
        public override void Open()
        {
            PackUI.OnPackRewardClicked += PurchasePack;
            InitRewardScreen();
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            PackUI.OnPackRewardClicked -= PurchasePack;
            gameObject.SetActive(false);
        }

        #endregion

        #region Purchase
        public void PurchasePack(PackRewardSO packRewardSO)
        {
            var diamondCost = packRewardSO.PurchaseCosts;
            ushort price = diamondCost[0].Price;
            if (Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.ReduceValue(price))
            {
                _lastPack = packRewardSO;
                _recievePack.Open(packRewardSO);
            }
        }

        public void RePurchasePack()
        {
            _recievePack.RecievePack();
            PurchasePack(_lastPack);
        }
        #endregion
    }
}
