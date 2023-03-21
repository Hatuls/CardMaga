using CardMaga.Tutorial;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class TutorialLineVisualHandler : MonoBehaviour, IInitializable<BadgeState>
    {
        [SerializeField] private Animator _animator;
        private static int OffAnimationHash = Animator.StringToHash("Tutorial_Line_Off_Anim");
        private static int CompletedAnimationHash = Animator.StringToHash("Tutorial_Line_Completed_Anim");
        private static int CurrentAnimationHash = Animator.StringToHash("Tutorial_Line_Open_Anim");
        public void CheckValidation()
        {
            if (_animator == null)
                throw new System.Exception("TutorialLineVisualHandler has no animator");
        }

        public void Dispose()
        {

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
                    throw new System.Exception($"TutorialLineVisualHandler has received wrong state: {comboData}");
            }
        }
    }




}