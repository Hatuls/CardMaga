﻿using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.Input
{
    public class Button : TouchableItem
    {
        [SerializeField] protected TMP_Text _buttonText;
        [SerializeField] protected Image _renderer;

        [Header("Sprites")]
        [SerializeField] protected Sprite _onPress;

        [OnValueChanged("ChangeToIdle")]
        [SerializeField] protected Sprite _onIdle;

        protected override void PointDown()
        {
            if (_onPress == null)
                return;

            _renderer.sprite = _onPress;
            base.PointDown();
        }
        protected override void PointUp()
        {
            if (_onIdle == null)
                return;

            _renderer.sprite = _onIdle;
            base.PointUp();
        }

        [Button("Toggle Text")]
        public void ToggleActiveState()
        {
            if (_buttonText == null)
                return;

            _buttonText.gameObject.SetActive(!_buttonText.gameObject.activeSelf);
        }

        public void SetButtonText(string text)
        {
            if (_buttonText == null || _buttonText.text.Equals(text))
                return;

            _buttonText.text = text;
        }


        #region Editor
#if UNITY_EDITOR
        [Header("Editor:")]
        [TextArea]
        [OnValueChanged("UpdateText")]
        [PropertyOrder(10)]

        public string Text;
        private void UpdateText() => SetButtonText(Text);
        private void ChangeToIdle()
        {
            if (_renderer != null)
                _renderer.sprite = _onIdle;
        }
#endif
        #endregion
    }
}

