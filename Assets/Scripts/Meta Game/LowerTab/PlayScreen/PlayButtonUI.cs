using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayButtonUI:ButtonUI
    {
        
        [SerializeField]
        Animator _animator;
        [SerializeField]
        float _cycleDuration = 1f;
        float _counter;
        int _shineAnimationHash = Animator.StringToHash("ToShine");
        private void Start()
        {
            FinishedCycle();
        }
        private void Update()
        {
            CycleCounter();
        }

        private void CycleCounter()
        {
            if (_counter >= _cycleDuration)
            {
                FinishedCycle();
                _counter = 0;
            }
            else
                _counter += Time.deltaTime;
        }

        public void FinishedCycle()
        {
            _animator.SetTrigger(_shineAnimationHash);
        }

        public void ResetAnimation()
        { 

            _animator.ResetTrigger(_shineAnimationHash);
        }
    }
}
