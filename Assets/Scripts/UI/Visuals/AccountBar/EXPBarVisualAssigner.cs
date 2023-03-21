using CardMaga.MetaData;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class EXPBarVisualAssigner : BaseVisualAssigner<AccountBarVisualData>
    {
        public event Action OnAccountBarVisualExpAtMax;

        [SerializeField] Slider _expSlider;
        [SerializeField] TransitionPackSO _accountSliderLevelUpTransitionSo;
        [SerializeField] TransitionPackSO _accountSliderMovementTransitionSo;
        Sequence _sliderMovementSequence;
        public override void CheckValidation()
        {
            if (_expSlider == null)
                throw new System.Exception("EXPBarVisualAssigner has no slider");
        }

        public override void Dispose()
        {
            _expSlider.maxValue = ZERO;
            _expSlider.minValue = ZERO;
            _expSlider.value = ZERO;
        }

        public override void Init(AccountBarVisualData comboData)
        {
            SetSliderMaxEXP(comboData.MaxExpAmount);
            SetSliderCurrentEXP(comboData.CurrentExpAmount);
        }

        public void SetSliderMaxEXP(int maxExp)
        {
            _expSlider.maxValue = maxExp;
        }
        public void SetSliderCurrentEXP(int currentEXP)
        {
            _expSlider.value = currentEXP;
        }

        public void AddSliderValue()
        {

        }
        private void MoveSliderToMax()
        {
            //when reciving enough exp to level up call this method that will
            //release an event that you completed visual transition
            _sliderMovementSequence.Join(_expSlider.DOValue(
                _expSlider.maxValue, _accountSliderLevelUpTransitionSo.Movement.TimeToTransition)
                ).OnComplete(OnAccountBarVisualExpAtMax.Invoke);
        }
    }
}