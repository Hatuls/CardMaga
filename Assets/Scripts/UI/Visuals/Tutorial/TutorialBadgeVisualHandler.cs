using CardMaga.Tutorial;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class TutorialBadgeVisualHandler : MonoBehaviour, IInitializable<BadgeState>
    {
        [SerializeField] private Animator _animator;
        [SerializeField] TutorialLineVisualHandler _tutorialLineVisualHandler;

        private static int OffAnimationHash = Animator.StringToHash("Tutorial_Badge_Off_Animation");
        private static int CompletedAnimationHash = Animator.StringToHash("Tutorial_Badge_Complete_Animation");
        private static int CurrentAnimationHash = Animator.StringToHash("Tutorial_Badge_Open_Animation");
        public void CheckValidation()
        {
            if (_animator == null)
                throw new System.Exception("TutorialBadgeVisualHandler has no animator");
            if (_tutorialLineVisualHandler == null)
                throw new System.Exception("TutorialLineVisualHandler");
            _tutorialLineVisualHandler.CheckValidation();
        }

        public void Dispose()
        {
            _tutorialLineVisualHandler.Dispose();
        }

        public void Init(BadgeState comboData)
        {
            switch (comboData)
            {
                case BadgeState.Off:
                    _animator.Play(OffAnimationHash);
                    break;
                case BadgeState.Open:
                    _animator.Play(CurrentAnimationHash);
                    break;
                case BadgeState.Complete:
                    _animator.Play(CompletedAnimationHash);
                    break;
                default:
                    throw new System.Exception($"TutorialBadgeVisualHandler has received wrong state: {comboData}");
            }
            _tutorialLineVisualHandler.Init(comboData);
        }
    }
}