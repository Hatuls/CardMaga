using CardMaga.UI.Account;
using CardMaga.UI.PopUp;
using UnityEngine;
namespace CardMaga.MetaUI.LevelUp
{

    public class MetaLevelUpManager : BasePopUpTerminal
    {
        [SerializeField]
        private AccountBarVisualManager _accountBarVisualManager;
        private LevelManager _levelManager;
        protected override void Start()
        {
            var instance = Account.AccountManager.Instance;
            if (instance == null)
                return;
            base.Start();
            _levelManager = instance.LevelManager;
            _levelManager.OnLevelUp += LevelUp;
            LevelUp();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            var instance = Account.AccountManager.Instance;
            if (instance == null)
                return;
            

            _levelManager.OnLevelUp -= LevelUp;
        }
        private void LevelUp()
        {
            _accountBarVisualManager.Refresh();
            var rewards = _levelManager.GetRewards();
            if (rewards.Length == 0)
                return;
            ShowPopUp();
            _currentActivePopUp.GetComponent<LevelUpRewardPopUp>().Init(rewards, HidePopUp);
        }

        protected override Vector2 GetStartLocation()
      => PopUpManager.Instance.GetPosition(_startLocation);
    }
}