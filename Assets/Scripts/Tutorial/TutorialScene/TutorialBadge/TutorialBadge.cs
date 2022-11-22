using CardMaga.Input;
using CardMaga.UI.Visuals;
using UnityEngine;

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

        public void Init()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Off);
        }

        public void Completed()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Complete);
            _Input.Lock();
        }

        public void Open()
        {
            _tutorialBadgeVisualHandler.Init(BadgeState.Open);
            _Input.UnLock();
        }
    }
}
