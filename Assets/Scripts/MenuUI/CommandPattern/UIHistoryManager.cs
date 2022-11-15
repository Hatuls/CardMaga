﻿using System;
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
    public abstract class BaseUIElement : MonoBehaviour, IUIElement
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;
        [SerializeField, Tooltip("The GameObjects that will be turning on and off\nIf left empty it will close the gameobject this script is on")]
        private GameObject _gameObject;
        public GameObject HolderGameObject
        {
            get
            {
                if (_gameObject == null)
                    _gameObject = gameObject;
                return _gameObject;
            }
        }
        public bool IsActive()
        => HolderGameObject.activeSelf || HolderGameObject.activeInHierarchy;
        public void Hide()
        {
            OnHide?.Invoke();
            HolderGameObject.SetActive(false);

        }

        public virtual void Init()
          => OnInitializable?.Invoke();


        public void Show()
        {
            OnShow?.Invoke();
            HolderGameObject.SetActive(true);
        }
    }
}