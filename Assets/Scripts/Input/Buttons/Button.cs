using Sirenix.OdinInspector;
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

        private IButtonVisualBehaviour _buttonVisualBehaviour;
        public virtual IButtonVisualBehaviour ButtonVisualBehaviour { get => _buttonVisualBehaviour; set => _buttonVisualBehaviour = value; }
        public Image Renderer => _renderer;
        protected override void Awake()
        {
            base.Awake();
            _buttonVisualBehaviour = new ChangeSpriteOnButtonLogic(_onPress, _onIdle);
        }
        protected override void PointDown()
        {
            if (_onPress == null)
                return;
            _buttonVisualBehaviour.VisualOnButtonPress(this);
            // _renderer.sprite = _onPress;
            base.PointDown();
        }
        protected override void PointUp()
        {
            if (_onIdle == null)
                return;

            //  _renderer.sprite = _onIdle;
            _buttonVisualBehaviour.VisualOnButtonUnPress(this);
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

    public interface IButtonVisualBehaviour
    {
        void VisualOnButtonPress(Button button);
        void VisualOnButtonUnPress(Button button);
    }

    public class ChangeSpriteOnButtonLogic : IButtonVisualBehaviour
    {

        private readonly Sprite onPress;
        private readonly Sprite onUnPress;

        public ChangeSpriteOnButtonLogic(Sprite onPress, Sprite onUnPress)
        {

            this.onPress = onPress;
            this.onUnPress = onUnPress;
        }
        public void VisualOnButtonPress(Button button)
        {
            button.Renderer.sprite = onPress;
        }

        public void VisualOnButtonUnPress(Button button)
        {
            button.Renderer.sprite = onUnPress;
        }
    }

    public class ChangeColorOnButtonLogic : IButtonVisualBehaviour
    {

        private readonly Color onPress;
        private readonly Color onUnPress;
        private readonly float scaleWhenPressed;
        private readonly float scaleWhenUnPressed;

        public ChangeColorOnButtonLogic(Color onPress, Color onUnPress, float scaleWhenPressed, float scaleWhenUnPressed)
        {
            this.onPress = onPress;
            this.onUnPress = onUnPress;
            this.scaleWhenUnPressed = scaleWhenUnPressed;
            this.scaleWhenPressed = scaleWhenPressed;
        }
        public void VisualOnButtonPress(Button button)
        {
            button.transform.localScale = scaleWhenPressed*Vector3.one;
            button.Renderer.color = onPress;
        }

        public void VisualOnButtonUnPress(Button button)
        {
            button.transform.localScale = scaleWhenUnPressed*Vector3.one;
            button.Renderer.color = onUnPress;
        }
    }
}

