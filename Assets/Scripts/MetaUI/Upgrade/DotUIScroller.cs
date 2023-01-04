using CardMaga.Input;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class DotUIScroller : MonoBehaviour
    {
        [SerializeField]
        private CardMaga.Input.Button[] _dotsImages;

       

        [SerializeField] UpgradeCardMover _upgradeCardMover;
        [SerializeField] UpgradeCardsDisplayer _upgradeCardsDisplayer;

        private int _currentFocusedIndex;
        [SerializeField]
        private Color _colorWhenPressed;
        [SerializeField]
        private Color _colorWhenUnPressed;
        [SerializeField]
        private float _scaleWhenPressed;
        [SerializeField]
        private float _scaleWhenUnPressed;
        private void OnEnable()
        {
            _upgradeCardsDisplayer.OnItemsIndexChanged += SetCurrentElement;
            _upgradeCardsDisplayer.OnItemCountChanged += AdjustDotsToSize;

            for (int i = 0; i < _dotsImages.Length; i++)
            {
                _dotsImages[i].ButtonVisualBehaviour = new ChangeColorOnButtonLogic(_colorWhenPressed, _colorWhenUnPressed, _scaleWhenPressed, _scaleWhenUnPressed);
                _dotsImages[i].ButtonVisualBehaviour.VisualOnButtonUnPress(_dotsImages[i]);
            }
        }
        private void OnDisable()
        {
            _upgradeCardsDisplayer.OnItemsIndexChanged -= SetCurrentElement;
            _upgradeCardsDisplayer.OnItemCountChanged -= AdjustDotsToSize;

        }
        public void Init(int maxElements, int currentElement)
        {
            SetCurrentElement(currentElement);
            AdjustDotsToSize(maxElements);
        }

        public void SetCurrentElement(int currentElement)
        {
            if (currentElement < 0 || currentElement >= _dotsImages.Length)
                return;
            _currentFocusedIndex = currentElement;
            Focus();
        }
        public void MoveOneRight()
        => SetCurrentElement(_currentFocusedIndex + 1);
        public void MoveOneLeft()
        => SetCurrentElement(_currentFocusedIndex - 1);
        private void Focus()
        {

            for (int i = 0; i < _dotsImages.Length; i++)
            {
#if UNITY_EDITOR
                _dotsImages[i].ButtonVisualBehaviour = new ChangeColorOnButtonLogic(_colorWhenPressed, _colorWhenUnPressed, _scaleWhenPressed, _scaleWhenUnPressed);
#endif

                if (i == _currentFocusedIndex)
                    _dotsImages[i].ButtonVisualBehaviour.VisualOnButtonPress(_dotsImages[i]);
                else
                    _dotsImages[i].ButtonVisualBehaviour.VisualOnButtonUnPress(_dotsImages[i]);
            }

        }

        private void AdjustDotsToSize(int dotsNeeded)
        {
            for (int i = 0; i < _dotsImages.Length; i++)
                _dotsImages[i].gameObject.SetActive(i < dotsNeeded);
        }


    }
}