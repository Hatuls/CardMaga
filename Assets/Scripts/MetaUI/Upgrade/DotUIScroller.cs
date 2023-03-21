using CardMaga.Input;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class DotUIScroller : MonoBehaviour
    {
        public event Action<int> OnDotClicked;
        [SerializeField]
        private Button[] _dotsButton;
        [SerializeField]
        private Dot[] _dotsUI;


        [SerializeField] UpgradeCardMover _upgradeCardMover;
        [SerializeField] UpgradeCardsDisplayer _upgradeCardsDisplayer;

        private int _currentElement;
        private int _currentFocusedIndex;

        [SerializeField]
        private Color _colorWhenPressedMainElement;
        [SerializeField]
        private Color _colorWhenUnPressedMainElement;

        [SerializeField]
        private Color _colorWhenPressed;
        [SerializeField]
        private Color _colorWhenUnPressed;
        [SerializeField]
        private float _scaleWhenPressed;
        [SerializeField]
        private float _scaleWhenUnPressed;

        private ChangeColorOnButtonLogic _notMainDotsBehaviour;
        private ChangeColorOnButtonLogic _mainDotsBehaviour;
        private void OnEnable()
        {

            _upgradeCardsDisplayer.OnItemsIndexChanged += SetCurrentElement;
            _upgradeCardsDisplayer.OnItemCountChanged += AdjustDotsToSize;
            InitDots();

        }

        private void InitDots()
        {
            _dotsUI = new Dot[_dotsButton.Length];
             _notMainDotsBehaviour = new ChangeColorOnButtonLogic(_colorWhenPressed, _colorWhenUnPressed, _scaleWhenPressed, _scaleWhenUnPressed);
            _mainDotsBehaviour = new ChangeColorOnButtonLogic(_colorWhenPressedMainElement, _colorWhenUnPressedMainElement, _scaleWhenPressed, _scaleWhenUnPressed);

            for (int i = 0; i < _dotsButton.Length; i++)
            {
                Button button = _dotsButton[i];
                Dot dot = new Dot(button, i);
     
                dot.OnDotPressed += DotPressed;
                _dotsUI[i] = dot;
            }
        }

        private void AssignBehaviours(int currentElement)
        {
            for (int i = 0; i < _dotsButton.Length; i++)
            {
                Button button = _dotsButton[i];
                button.ButtonVisualBehaviour = currentElement==i ? _mainDotsBehaviour : _notMainDotsBehaviour;
                button.ButtonVisualBehaviour.VisualOnButtonUnPress(button);
                button.UnLock();
            }
        }

        private void DotPressed(int index)
                    => _upgradeCardsDisplayer.SetView(index);

        private void OnDisable()
        {
            _upgradeCardsDisplayer.OnItemsIndexChanged -= SetCurrentElement;
            _upgradeCardsDisplayer.OnItemCountChanged -= AdjustDotsToSize;

            for (int i = 0; i < _dotsUI.Length; i++)
                _dotsUI[i].OnDotPressed -= DotPressed;
        }
        public void Init(int maxElements, int currentElement)
        {
            _currentElement = currentElement;
            SetCurrentElement(currentElement);
            AdjustDotsToSize(maxElements);
        }
          
        public void SetCurrentElement(int currentElement)
        {
            if (currentElement < 0 || currentElement >= _dotsUI.Length)
                return;
            _currentFocusedIndex = currentElement;
            AssignBehaviours(_currentElement);
            Focus();
        }

     
        public void MoveOneRight()
        => SetCurrentElement(_currentFocusedIndex + 1);
        public void MoveOneLeft()
        => SetCurrentElement(_currentFocusedIndex - 1);
        private async void Focus()
        {

#if UNITY_EDITOR
            if (_dotsUI == null || _dotsUI.Length == 0)
                InitDots();
#endif
            await Task.Yield();

            for (int i = 0; i < _dotsUI.Length; i++)
            {
                Button button = _dotsUI[i].Button;
             
                if (i == _currentFocusedIndex)
                    button.ButtonVisualBehaviour.VisualOnButtonPress(button);
                else
                    button.ButtonVisualBehaviour.VisualOnButtonUnPress(button);
                
            }

        }

        private void AdjustDotsToSize(int dotsNeeded)
        {
            var behaviour = new ChangeColorOnButtonLogic(_colorWhenPressed, _colorWhenUnPressed, _scaleWhenPressed, _scaleWhenUnPressed);
            for (int i = 0; i < _dotsUI.Length; i++)
            {
                _dotsUI[i].Button.gameObject.SetActive(i < dotsNeeded);
                _dotsUI[i].Button.ButtonVisualBehaviour = behaviour;
            }
        }



    }
  
    public class Dot
    {
        public event Action<int> OnDotPressed;
        private Button _button;
        public Button Button =>_button;
        private int _level;
        public Dot(Button button, int index)
        {
            this._button = button;
            this._level = index;
            _button.OnClick += OnClick;
        }

        ~Dot()
        {
            _button.OnClick -= OnClick;
        }
        private void OnClick() => OnDotPressed?.Invoke(_level);
    }
}