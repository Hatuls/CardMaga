using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;
namespace CardMaga.UI.Visuals
{
    public class TopPartArmorUI : MonoBehaviour
    {
        [Header("Testing")]
        public int Amount;
        [Button]
        public void TestArmorChange()
        {
            SetArmor(Amount);
        }

        [Header("Fields")]
        [SerializeField] TransitionPackSO _gainArmorTransitionSO;
        [SerializeField] TransitionPackSO _reduceArmorTransitionSO;
        [SerializeField] TransitionPackSO _breakArmorTransitionSO;
        [SerializeField] TextMeshProUGUI _armorAmountText;
        [SerializeField] RectTransform _scalerRectTransform;
        [SerializeField] RectTransform _breakArmorRectTransform;
        
        int _currentArmor;
        bool _canArmorReset = true;
        bool _isReducedByHalf = false;
        public bool IsArmorResets { set => _canArmorReset = value; }
        public bool IsReducedByHalf { set => _isReducedByHalf = value; }

        private void Awake()
        {
            if (_scalerRectTransform == null)
                throw new Exception("TopPartArmorUI has no ArmorImageIsNull");
            if (_breakArmorRectTransform)
                Debug.LogWarning("TopPartArmorUI has no BreakArmorImage");
            if (_armorAmountText == null)
                throw new Exception("TopPartArmorUI has no armor text");
        }
        
        public void SetArmor(int amount)
        {
            if (amount > _currentArmor)
            {
                GainArmor(amount);
            }
            else if(amount < _currentArmor)
            {
                ReduceArmor(amount);
            }
            else
            {
                Debug.LogWarning("Armor did not change but you called Set Armor");
            }
        }
        public void ResetArmor()
        {
            if (_currentArmor == 0)
            {
                Debug.Log("Armor is already 0 no need to change it");
                return;
            }
            if (_canArmorReset)
            {
                _currentArmor = 0;
            }
            else if (_isReducedByHalf)
            {
                float half = _currentArmor / 2;

                _currentArmor = (int)Mathf.Ceil(half);
            }
            else
            {
                Debug.Log("Armor should not reset");
            }
        }
        private void GainArmor(int amount)
        {
            if (_currentArmor == 0)
            {
                ActivateArmor();
            }
            _currentArmor = amount;
            SetText(_currentArmor);
            GainArmorAnimation();
        }
        private void ReduceArmor(int amount)
        {
            if (amount <= 0)
            {
                _currentArmor = 0;
                //we have 0 or less armor
                BreakArmorAnimation();
                DeactivateArmor();
            }
            else
            {
                _currentArmor = amount;
            }
            SetText(_currentArmor);
            ReduceArmorAnimation();
        }
        private void ReduceArmorAnimation()
        {
            _scalerRectTransform.Transition(_reduceArmorTransitionSO);
        }
        private void GainArmorAnimation()
        {
            _scalerRectTransform.Transition(_gainArmorTransitionSO);
        }
        private void SetText(int amount)
        {
            _armorAmountText.text = string.Concat(amount);
        }
        private void ActivateArmor()
        {
            //need implementation
        }
        private void DeactivateArmor()
        {
            //need implementation
        }
        private void BreakArmorAnimation()
        {
            //shatter animation
        }
    }
}
