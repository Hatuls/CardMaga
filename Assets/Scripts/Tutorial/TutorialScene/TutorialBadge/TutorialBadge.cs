using CardMaga.Input;
using CardMaga.UI.Visuals;
using UnityEngine;
using System;

namespace CardMaga.Tutorial
{
    public enum BadgeState
    {
        Off = 0,
        Open = 1,
        Complete = 2,
    }
    public class TutorialBadge :MonoBehaviour
    {
        [SerializeField] TutorialBadgeVisualHandler _tutorialBadgeVisualHandler;
        [SerializeField] private TouchableItem _Input;
        [SerializeField] public TutorialConfigSO _configSO;

        public event Action<TutorialConfigSO> OnBadgeClicked;

        public void BadgeClicked()
        {
            if (OnBadgeClicked != null)
                OnBadgeClicked.Invoke(_configSO);
        }

        public void Init()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Off);
        }

        public void Completed()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Complete);
            _Input.UnLock();
        }

        public void Open()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Open);
            _Input.UnLock();
        }
    }
}
