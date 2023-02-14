using CardMaga.UI.Account;
using UnityEngine;
namespace CardMaga.MetaUI.LevelUp
{

    public class MetaLevelUpManager : MonoBehaviour
    {
        [SerializeField]
        private AccountBarVisualManager _accountBarVisualManager;
        private LevelManager _levelManager;
        private void Start()
        {
            var instance = Account.AccountManager.Instance;
            if (instance == null)
                return;

            _levelManager = instance.LevelManager;

            _levelManager.OnLevelUp += LevelUp;
            LevelUp();
        }
        private void OnDestroy()
        {
            var instance = Account.AccountManager.Instance;
            if (instance == null)
                return;


            _levelManager.OnLevelUp -= LevelUp;
        }
        private void LevelUp()
        {
            Rewards.RewardManager.Instance.OpenRewardsScene(_levelManager.GetRewardsList());
            _accountBarVisualManager.Refresh();
        }
    }

}