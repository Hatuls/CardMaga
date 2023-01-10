using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI
{

    public static class UIHistoryManager
    {
        private static Stack<IUIElement> _history = new Stack<IUIElement>();
        private static IUIElement _currentUIElement;

        public static bool IsEmpty => _history.Count == 0;

        public static void Show(IUIElement showable, bool toRemember)
        {
            if (showable == _currentUIElement)
                return;

            if (_currentUIElement != null)
            {
                if (toRemember)
                    _history.Push(_currentUIElement);


                _currentUIElement.Hide();
            }

            showable.Show();

            _currentUIElement = showable;
        }

        public static void ReturnBack()
        {
            if (!IsEmpty)
                Show(_history.Pop(), false);
            else if (_currentUIElement != null)
            {
                _currentUIElement.Hide();
                _currentUIElement = null;
            }

        }

        public static void CloseAll()
        {
            while (!IsEmpty)
                ReturnBack();
            if (_currentUIElement != null)
            {
                _currentUIElement.Hide();
                _currentUIElement = null;
            }
        }


    }


    public abstract class BaseUIScreen : BaseUIElement
    {
        [SerializeField]
        private bool _toRememberWhenOpenScreen = true;

        public virtual void OpenScreen()
        {
            UIHistoryManager.Show(this, _toRememberWhenOpenScreen);
        }
        public virtual void CloseScreen()
        {
            UIHistoryManager.ReturnBack();
        }
    }
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;
        [Sirenix.OdinInspector.PropertyOrder(-1000) ,SerializeField, Tooltip("The RectTransform of the object\nIf left empty it will try to use this object's recttransfrom")]
        private RectTransform _rectTransform;
        [Sirenix.OdinInspector.PropertyOrder(-1000) ,SerializeField, Tooltip("The GameObjects that will be turning on and off\nIf left empty it will close the gameobject this script is on")]
        private GameObject _holderGameObject;
        public GameObject HolderGameObject
        {
            get
            {
                if (_holderGameObject == null)
                    _holderGameObject = gameObject;
                return _holderGameObject;
            }
        }

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform==null && !TryGetComponent<RectTransform>(out _rectTransform))
                        throw new Exception($"This UI Element {gameObject.name} is not an UI element and need to have a recttransfrom or to have a reference to a recttransform!");

                
                return _rectTransform;
            }

        }


        public bool IsActive()
        => HolderGameObject.activeSelf || HolderGameObject.activeInHierarchy;
        public virtual void Hide()
        {
            OnHide?.Invoke();
            HolderGameObject.SetActive(false);
        }

        public virtual void Init()
          => OnInitializable?.Invoke();


        public virtual void Show()
        {
            OnShow?.Invoke();
            HolderGameObject.SetActive(true);
        }
    }
}