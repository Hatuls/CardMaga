using CardMaga.Rewards;
using System;
using UnityEngine;
namespace CardMaga.MetaUI.LevelUp
{
    public class LevelUpRewardPopUp : MonoBehaviour
    {
        private BaseRewardFactorySO[] _baseRewardFactorySOs;
            private Action OnConfirm;
        internal void Init(BaseRewardFactorySO[] baseRewardFactorySOs, Action hidePopUp)
        {
            OnConfirm = hidePopUp;
            _baseRewardFactorySOs = baseRewardFactorySOs;
        }

        public void OpenRewardScreen()
        {
            RewardManager.Instance.OpenRewardsScene(_baseRewardFactorySOs);
            OnConfirm?.Invoke();
        }
    }
}