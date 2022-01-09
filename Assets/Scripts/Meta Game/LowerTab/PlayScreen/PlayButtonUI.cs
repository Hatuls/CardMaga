using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayButtonUI:ButtonUI
    {
        
        [SerializeField]
        Animator _animator;
   
        int _shineAnimationHash = Animator.StringToHash("ToShine");
        int _clickAnimationHash = Animator.StringToHash("OnClick");
        private void Start()
        {
            FinishedCycle();
        }

        public void FinishedCycle()
        {
            _animator.SetTrigger(_shineAnimationHash);
        }
        public void OnClickAnimation() => _animator.SetTrigger(_clickAnimationHash);
        public void ResetAnimation()
        { 

            _animator.ResetTrigger(_shineAnimationHash);
        }
    }
}
