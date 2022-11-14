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

            else
                _currentUIElement?.Hide();

        }

        public static void CloseAll()
        {
            while (!IsEmpty)
                ReturnBack();

            _currentUIElement?.Hide();
        }


    }
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;
        [SerializeField, Tooltip("The GameObjects that will be turning on and off\nIf left empty it will close the gameobject this script is on")]
        private GameObject _gameObject;
        public GameObject PopupGameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = gameObject;
                return _gameObject;
            }
        }
        public bool IsActive()
        => PopupGameObject.activeSelf || PopupGameObject.activeInHierarchy;
        public void Hide()
        {
            OnHide?.Invoke();
            PopupGameObject.SetActive(false);
     
        }

        public virtual void Init()
          => OnInitializable?.Invoke();


        public void Show()
        {
            OnShow?.Invoke();
            PopupGameObject.SetActive(true);
        }
    }
}