using Rewards;
using Rewards.Packs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] Animator _animator;

        private PackRewardSO _lastPack;
        #region Initialize
        private void InitRewardScreen()
        {
            PackRewardSO[] packs = _packRewardsCollectionSO.PacksRewardSO;
            int length = packs.Length;


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

        [SerializeField]
        GameObject[] ContrainersToActivate;
        public override void Open()
        {
            PackUI.OnPackRewardClicked += PurchasePack;
            InitRewardScreen();
            for (int i = 0; i < ContrainersToActivate.Length; i++)
            {
                ContrainersToActivate[i].SetActive(true);
            }

        }
        public override void Close()
        {
            PackUI.OnPackRewardClicked -= PurchasePack;
            for (int i = 0; i < ContrainersToActivate.Length; i++)
            {
                ContrainersToActivate[i].SetActive(false);
            }
        }

        #endregion

        #region Purchase
        [SerializeField]
        UnityEvent OnSuccessfullPurchase;
        [SerializeField]
        UnityEvent OnUnSuccessfullPurchase;
        public void PurchasePack(PackRewardSO packRewardSO)
        {
            var diamondCost = packRewardSO.PurchaseCosts;
            ushort price = diamondCost[0].Price;
            _animator.Play("Default");
            if (Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.ReduceValue(price))
            {
                OnSuccessfullPurchase?.Invoke();
                _lastPack = packRewardSO;
                SendAnalyticEvent();
                _animator.SetTrigger("Pop");
                _recievePack.Open(packRewardSO);
            }
            else
                OnUnSuccessfullPurchase?.Invoke();
        }

        private void SendAnalyticEvent()
        {
            AnalyticsHandler.SendEvent(string.Concat(_lastPack.PackName, "_purchased"));
            FireBaseHandler.SendEvent(string.Concat(_lastPack.PackName, "_purchased"));
        }

        public void RePurchasePack()
        {

            _recievePack.RecievePack();
            PurchasePack(_lastPack);
        }
        #endregion
    }
}
