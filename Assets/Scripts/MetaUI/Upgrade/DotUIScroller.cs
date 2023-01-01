using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardMaga.Meta.Upgrade
{
    public class DotUIScroller : MonoBehaviour
    {
        [SerializeField]
        private Image[] _dotsImages;

        [SerializeField]
        private DotUIChanges _whenFocused;
        [SerializeField]
        private DotUIChanges _whenNotFocused;


        private int _currentFocusedIndex;
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
                if (i == _currentFocusedIndex)
                    _whenFocused.Apply(_dotsImages[i]);
                else
                    _whenNotFocused.Apply(_dotsImages[i]);
            }

        }

        private void AdjustDotsToSize(int dotsNeeded)
        {
            for (int i = 0; i < _dotsImages.Length; i++)
                _dotsImages[i].gameObject.SetActive(i <= dotsNeeded);
        }

        [Serializable]
        public class DotUIChanges
        {
            [SerializeField]
            private float _scale = 1f;
            [SerializeField]
            private Color _color = Color.white;
            [SerializeField]
            private Sprite _sprite;

            public void Apply(Image img)
            {
                img.transform.localScale = _scale * Vector3.one;
                img.color = _color;
                if (_sprite != null)
                    img.sprite = _sprite;

            }
        }
    }
}